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
    public partial class Popup_EditProject : System.Web.UI.Page
    {
        protected static DataSet warehousesItems;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                warehousesItems = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
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
                DataTable gridTable = new DataTable("WareHouses");
                gridTable.Columns.Add("Selected");
                gridTable.Columns.Add("No");
                gridTable.Columns.Add("Code");
                gridTable.Columns.Add("Name");
                gridTable.Columns.Add("SrvType");
                gridTable.Columns.Add("TimeEndDate");
                DataTable warehouseTable = warehousesItems.Tables[0];
                DataRow dr;
                int i = 0;
                foreach (DataRow row in warehouseTable.Rows)
                {
                    if (("" + row[0].ToString().ToUpper() + row[1].ToString().ToUpper() ).Trim().IndexOf(CategoryFilter.Trim().ToUpper()) >= 0)
                    {
                        dr = gridTable.NewRow();
                        if (i == 0)
                            dr["Selected"] = "checked";
                        else
                            dr["Selected"] = "";
                        dr["No"] = i.ToString(); warehouseTable.Rows.IndexOf(row);
                        dr["Code"] = row[0].ToString();
                        dr["Name"] = row[1].ToString();
                        dr["SrvType"] = row["U_AI_SrvType"].ToString();
                        dr["TimeEndDate"] = row["U_AI_TimeEndDate"].ToString();
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
                List<Project> list = Project.extractFromDataSet(warehousesItems.Tables[0]);
                Project chosenProject = list[Int32.Parse(selectedValue)];
                Session["chosenProject"] = chosenProject;
                Session["chosenItemNo"] = Request.QueryString["id"];
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OKWareHousePopup", "Main.okDialogClick('EditProjectCallBack');", true);

        }
    }
}