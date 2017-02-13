using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAP.WebServices;
using SAP.Admin.DAO;
using System.Collections;

namespace SAP
{
    public partial class Popup_MultiProject : System.Web.UI.Page
    {
        protected static DataSet itemMasters;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Project masterDataWS = new Project();
                itemMasters = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
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
                DataTable gridTable = new DataTable("Items");
                gridTable.Columns.Add("Selected");
                gridTable.Columns.Add("No");
                gridTable.Columns.Add("Code");
                gridTable.Columns.Add("Name");
                DataTable itemsTable = itemMasters.Tables[0];
                DataRow dr;
                int i = 0;
                foreach (DataRow row in itemsTable.Rows)
                {
                    if (("" + row[0].ToString() + row[1].ToString()).Trim().ToUpper().IndexOf(CategoryFilter.Trim().ToUpper()) >= 0)
                    {
                        dr = gridTable.NewRow();
                        dr["No"] = i.ToString();
                        itemsTable.Rows.IndexOf(row);
                        dr["Code"] = row[0].ToString();
                        dr["Name"] = row[1].ToString();
                        gridTable.Rows.Add(dr);
                    }
                    i++;
                }
                gridTable.Rows[gridTable.Rows.Count - 1]["Selected"] = "checked";
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
            List<Project> list = Project.extractFromDataSet(itemMasters.Tables[0]);

            String[] SelectedNo = Request.Form["chk"].ToString().Split(',');
            ArrayList checkedItems = new ArrayList();

            for (int i = 0; i <= SelectedNo.Length - 1; i++)
            {
                Project chosenItem = list[int.Parse(SelectedNo[i])];
                checkedItems.Add(chosenItem);
            }

            Session["chosenProject"] = checkedItems;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OKPopup", "Main.okDialogClick('MultiProjectCallBack');", true);
        }
    }
}
