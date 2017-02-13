using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Collections;

namespace SAP.Admin
{
    public partial class SystemUsers : System.Web.UI.Page
    {
        #region Control
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowUserList();
            }
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            ShowUserList();
        }

        protected void systemUsersListView_PageIndexChanged(object sender, EventArgs e)
        {
            ShowUserList();
        }

        protected void systemUsersListView_Sorting(object sender, GridViewSortEventArgs e)
        {
            ShowUserList();
        }

        protected void systemUsersListView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            systemUsersListView.DataBind();

            ShowUserList();
        }

        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetSystemUserEditUrl(""));
        }
        #endregion
        #region functions

        private void ShowRoleList()
        {
            try
            {
                string[] roleNames = Roles.GetAllRoles();
                string userName = string.Empty;
                if (Context.User != null)
                {
                    userName = Context.User.Identity.Name;
                }
            }
            catch (Exception ex)
            {
  
            }
        }

        private void ShowUserList()
        {
            try
            {
                //Show system user as normal
                MembershipUserCollection list = new MembershipUserCollection();
                string userNameFilter = userNameTextBox.Text.Trim().ToUpper();
                MembershipUserCollection users = Membership.GetAllUsers();

                try
                {
                    foreach (MembershipUser user in users)
                    {                        
                        if (string.IsNullOrEmpty(userNameFilter))
                        {
                            list.Add(user);
                        }
                        else if (user.UserName.ToUpper().Contains(userNameFilter))
                        {
                            list.Add(user);
                        }
                    }
                }
                catch { }

                systemUsersListView.DataSource = list;
                systemUsersListView.DataBind();
                //DataBindGridView(ConvertToDataTable(list));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                                                    "Main.setMasterMessage('No record found!','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading",
                                                    "Dialog.hideLoader();", true);
            }
        }

        private void DataBindGridView(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                systemUsersListView.DataSource = null;
            }
            else
            {
                systemUsersListView.DataSource = table;
            }
            systemUsersListView.DataBind();
        }

        private DataTable ConvertToDataTable(MembershipUserCollection userList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("UserName");
            table.Columns.Add("Email");

            IEnumerator iList = userList.GetEnumerator();
            string currentUserName = "";

            if (HttpContext.Current.User.Identity != null)
            {
                currentUserName = HttpContext.Current.User.Identity.Name.ToLower().Trim();
            }

            while (iList.MoveNext())
            {
                string userName = ((MembershipUser)(iList.Current)).UserName;
                //if ((currentUserName != "admin") && (userName.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase) || currentUserName.Equals(userName.Trim(), StringComparison.OrdinalIgnoreCase)))
                //{
                //    continue; // remove currentUser to the list 
                //}
                string email = ((MembershipUser)(iList.Current)).Email;

                DataRow row = table.NewRow();
                row["UserName"] = userName;
                row["Email"] = email;
                table.Rows.Add(row);
            }
            return table;
        }

        protected string GetSystemUserEditUrl(string userName)
        {
            return string.Format("SystemUserEdit.aspx?UserName={0}", userName);
        }
        #endregion

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~/") + filename); // Save a file in server.
                    
                    oExcel ox = new oExcel(Server.MapPath("~/") + filename); 
                    DataTable dt = ox.GetDataSQL("Select * from [Sheet1$]");
                    oUsers ou = new oUsers();

                    foreach (DataRow dr in dt.Rows)
                    {
                        ou.CreateUser(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString()); // 0: UserCode, 1: Password, 2: Roll Name, 3: Email
                    }
                    StatusLabel.Text = "Import status: File had imported.";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Import status: The file could not be imported. The following error occured: " + ex.Message;
                }
            }
        }
    }
}