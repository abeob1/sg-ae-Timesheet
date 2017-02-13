using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SAP.WebServices;
using System.Collections;
using System.Globalization;
using System.Threading;
using SAP.Admin.DAO;

namespace SAP
{
    public partial class ProjectMandaysUsedReport : System.Web.UI.Page
    {
        private static DataSet ds = new DataSet();
        private static string msCompany = string.Empty, msProject = string.Empty, msToDate = DateTime.Now.ToString("MM/dd/yyyy");
        private static DateTime Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday;
        private static bool mb_Export2Excel = false;
        private static DataTable mdtExport2Xls = new DataTable();

        #region Events

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Binding();
            }
        }
        #endregion

        #region btnFilterMyTS_Click
        protected void btnFilterMyTS_Click(object sender, EventArgs e)
        {
            msToDate = txtToDate.Text;
            msProject = txtProjectCode.Text.Trim();
            msCompany = ddlCompany.SelectedValue;
            Binding();
        }
        #endregion

        #region btnExport2Xlsx_Click
        protected void btnExport2Xlsx_Click(object sender, EventArgs e)
        {
            mb_Export2Excel = true;
            BuuildDT();
            Response.Redirect(Request.RawUrl);
        }
        #endregion

        #endregion

        #region Override Events

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            SetForm();
            if (mb_Export2Excel) ExportToExcelWithFormat();
            if (this.Request["__EVENTARGUMENT"] != null && this.Request["__EVENTARGUMENT"].ToString() != "")
            {
                Int32 itemNo = 0;
                Project chosenPrj = null;
                switch (this.Request["__EVENTARGUMENT"].ToString())
                {
                    case "EditProjectCallBack":
                        chosenPrj = Session["chosenProject"] as Project;
                        itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                        if (chosenPrj != null)
                        {
                            txtProjectCode.Text = chosenPrj.PrjCode;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion

        #region Methods

        #region Binding
        protected void Binding()
        {
            txtToDate.Text = msToDate;
            txtProjectCode.Text = msProject;

            CultureInfo ivC = new CultureInfo("es-US");
            DateTime Today = Convert.ToDateTime(msToDate, ivC);
            Monday = Today.AddDays(1 - Today.DayOfWeek.GetHashCode());
            Tuesday = Today.AddDays(2 - Today.DayOfWeek.GetHashCode());
            Wednesday = Today.AddDays(3 - Today.DayOfWeek.GetHashCode());
            Thursday = Today.AddDays(4 - Today.DayOfWeek.GetHashCode());
            Friday = Today.AddDays(5 - Today.DayOfWeek.GetHashCode());
            Saturday = Today.AddDays(6 - Today.DayOfWeek.GetHashCode());
            Sunday = Today.AddDays(7 - Today.DayOfWeek.GetHashCode());

            ////////////////////////////////////////////////////////////////////
            DataSet ldsDBName = LoadDBName();
            ddlCompany.DataSource = ldsDBName.Tables[0];
            ddlCompany.DataTextField = "DBName";
            ddlCompany.DataValueField = "DBName";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, "");
            ListItem item = ddlCompany.Items.FindByValue(msCompany);
            ddlCompany.SelectedValue = item.Value;
            
            /////////////////////////////////////////////////////////////////////

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectMandaysUsedReport",
                Data.CreateParameter("@IN_BeginDate", Today.ToString("MM/dd/yyyy")),
                //Data.CreateParameter("@IN_EndDate", Sunday.ToString("MM/dd/yyyy")),
                Data.CreateParameter("@IN_DBName", ddlCompany.SelectedValue),
                Data.CreateParameter("@IN_ProjectCode", txtProjectCode.Text.Trim()));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }
        #endregion

        private DataSet LoadDBName()
        {
            return SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBList");
        }

        #region SetForm
        private void SetForm()
        {
            for(int i = 0; i < lvStage.Items.Count ; i++)
            {
                ListViewItem lvi = lvStage.Items[i];

                if (ds.Tables[0].Rows[i]["Total"].ToString().Equals("True"))
                {
                    ((Label)lvi.FindControl("PrjCode")).Font.Bold = 
                    ((Label)lvi.FindControl("PrjName")).Font.Bold = 
                    ((Label)lvi.FindControl("UserCode")).Font.Bold =
                    ((Label)lvi.FindControl("MandaysUtilized")).Font.Bold = true;
                }
            }
        }
        #endregion

        #region ExportToExcelWithFormat
        private void ExportToExcelWithFormat()
        {
            DataGrid dg = new DataGrid();
            dg.DataSource = mdtExport2Xls;
            dg.DataBind();
            FillDataToExcel("ProjectMandaysUsedReport.xls", dg);
            dg = null;
            dg.Dispose();
        }
        #endregion

        #region FillDataToExcel
        private void FillDataToExcel(string strFileName, DataGrid dg)
        {
            mb_Export2Excel = false;
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            Response.ContentType = "application/msexcel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        #endregion

        #region BuuildDT
        private void BuuildDT()
        {   //Create Tempory Table
            mdtExport2Xls = new DataTable();
            //Creating Header Row
            mdtExport2Xls.Columns.Add("<b>Project Code</b>");
            mdtExport2Xls.Columns.Add("<b>Project Name</b>");
            mdtExport2Xls.Columns.Add("<b>Consultant</b>");
            mdtExport2Xls.Columns.Add("<b>Man-days Utilized</b>");

            DataRow drAddItem;
            Decimal ldec = 0;
            int li = 0;
            foreach (ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();

                if (((Label)lvi.FindControl("lblTotal")).Text.Equals("False"))
                {
                    drAddItem[0] = ((Label)lvi.FindControl("PrjCode")).Text;            // Project Code
                    drAddItem[1] = ((Label)lvi.FindControl("PrjName")).Text;            // Project Name
                    drAddItem[2] = ((Label)lvi.FindControl("UserCode")).Text;           // Consultant
                    ldec = CS2Dec(((Label)lvi.FindControl("MandaysUtilized")).Text);    // Man-days Utilized
                    drAddItem[3] = ldec.ToString();
                }
                else
                {
                    drAddItem[0] = "<b>" + ((Label)lvi.FindControl("PrjCode")).Text + "</b>"; // Project Code
                    drAddItem[1] = "<b>" + ((Label)lvi.FindControl("PrjName")).Text + "</b>"; // Project Name
                    drAddItem[2] = "<b>" + ((Label)lvi.FindControl("UserCode")).Text + "</b>";// Consultant
                    ldec         = CS2Dec(((Label)lvi.FindControl("MandaysUtilized")).Text);  // Total Hrs.
                    drAddItem[3] = "<b>" + ldec.ToString() + "</b>"; 
                }
                mdtExport2Xls.Rows.Add(drAddItem);
                li++;
            }
        }
        #endregion

        #region CS2Dec
        private decimal CS2Dec(string asDec)
        {
            try
            {
                return decimal.Parse(asDec);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #endregion

    }
}