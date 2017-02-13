using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAP.WebServices;
using SAP.Admin.DAO;

namespace SAP
{
    public partial class Popup_UserList : System.Web.UI.Page
    {
        protected static DataSet dsUsers;
        protected static string querytype = string.Empty;

        #region QueryType
        public static string QueryType
        {
            get { return querytype; }
            set { querytype = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                QueryType = Request.QueryString["id"];
                if(QueryType.CompareTo("0") == 0) // Reporting Manager list
                    dsUsers = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, " select UserId, UserName, Email from vw_aspnet_MembershipUsers  where UserId in (select UserID from tbl_ProjectManager) ");
                else // Consultant list
                    dsUsers = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, " select UserId, UserName, Email from vw_aspnet_MembershipUsers");

                BindCategories("");
                editWareHouseUpdatePanel.Update();
            }
        }

        protected void txtCategoryNameHeader_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            BindCategories(text);
        }

        private void BindCategories(string CategoryFilter)
        {
            try
            {
                // Simple created a table to bind with Grid view and populated it with data.
                DataTable gridTable = new DataTable("Users");
                gridTable.Columns.Add("Selected");
                gridTable.Columns.Add("No");
                gridTable.Columns.Add("UserID");
                gridTable.Columns.Add("UserName");
                gridTable.Columns.Add("Email");

                DataTable userTable = dsUsers.Tables[0];
                DataRow dr;
                int i = 0;
                foreach (DataRow row in userTable.Rows)
                {
                    if (("" + row[0].ToString() + row[1].ToString()).Trim().IndexOf(CategoryFilter.Trim()) >= 0)
                    {
                        dr = gridTable.NewRow();
                        if (i == 0)
                            dr["Selected"] = "checked";
                        else
                            dr["Selected"] = "";
                        dr["No"] = i.ToString(); userTable.Rows.IndexOf(row);
                        dr["UserID"] = row["UserID"].ToString(); 
                        dr["UserName"] = row["UserName"].ToString();
                        dr["Email"] = row["Email"].ToString();
                        gridTable.Rows.Add(dr);
                    }
                    i++;
                }

                listWareHouses.DataSource = gridTable;
                listWareHouses.DataBind();
                editWareHouseUpdatePanel.Update();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindCategories(this.txtFilter.Text);
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            string selectedValue = Request.Form["MyRadioButton"];
            if (!String.IsNullOrEmpty(selectedValue))
            {
                List<UserList> list = UserList.extractFromDataSet(dsUsers.Tables[0]);
                UserList chosenUser = list[Int32.Parse(selectedValue)];
                Session["chosenUser"] = chosenUser;
                Session["chosenItemNo"] = string.IsNullOrEmpty(Request.QueryString["no"]) ? Request.QueryString["id"] : Request.QueryString["no"];
            }
            if (QueryType.CompareTo("0") == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OKWareHousePopup", "Main.okDialogClick('ReportingManagerCallBack');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OKWareHousePopup", "Main.okDialogClick('ConsultantFilterCallBack');", true);
        }
    }

    public class UserList
    {
        private string userid;
        private string username;
        private string email;

        #region UserID
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        #endregion

        #region UserName
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
        #endregion

        #region Email
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        #endregion

        #region extractFromDataSet
        public static List<UserList> extractFromDataSet(DataTable table)
        {
            List<UserList> list = new List<UserList>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    UserList user = new UserList();
                    user.UserID = row["UserID"].ToString();
                    user.UserName = row["UserName"].ToString();
                    user.Email = row["Email"].ToString();
                    list.Add(user);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return list;
        }
        #endregion
    }
}