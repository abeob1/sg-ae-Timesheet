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
using SAP.Admin.DAO;

namespace SAP
{
    public partial class TimeEntryMissingReport : System.Web.UI.Page
    {
        private static bool mb_Export2Excel = false;
        private static DataSet ds = new DataSet();
        private static DataTable mdtExport2Xls = new DataTable();
        private static string FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        private static string ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");
        private static DataSet dsDBName = new DataSet();
        private static int midx = -1;

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
            FromDate = txtFromDate.Text.Trim();
            ToDate = txtToDate.Text.Trim();
            midx = ddl_Company.SelectedIndex;
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
            //SetForm();
            if (mb_Export2Excel) ExportToExcelWithFormat();
        }

        #endregion

        #region Methods

        #region Binding
        protected void Binding()
        {
            txtFromDate.Text = FromDate;
            txtToDate.Text = ToDate;
            LoadDBName();
            if (ddl_Company.Items.Count > 0) ddl_Company.SelectedIndex = midx;

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_TimeEntrySumReport",
                Data.CreateParameter("@IN_BeginDate", txtFromDate.Text),
                Data.CreateParameter("@IN_EndDate", txtToDate.Text),
                Data.CreateParameter("@IN_SAPB1DB", ddl_Company.SelectedValue));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }
        #endregion

        #region SetForm
        private void SetForm()
        {
            for(int i = 0; i < lvStage.Items.Count ; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                if (ds.Tables[0].Rows[i]["UserCode"].ToString().Equals("ZZZZZZZZZZZZZZZZZZZZ"))
                {
                    int idx = lvi.Controls.IndexOf((Label)lvi.FindControl("Date"));
                    lvi.Controls.Remove((Label)lvi.FindControl("Date"));
                    ((Label)lvi.FindControl("Hours")).Font.Bold = true;
                    Label lblTotal = new Label();
                    lblTotal.ID = "lblTotal"+i.ToString();
                    if (i == lvStage.Items.Count - 1)
                    {
                        lblTotal.Text = "Grand Total:";
                        lblTotal.Font.Bold = true;
                        lvi.Controls.AddAt(idx, lblTotal);
                        ((Label)lvi.FindControl("PrjCode")).Text = string.Empty;
                    }
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
            FillDataToExcel("SummaryOfTimesheetApproved.xls", dg);
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
            mdtExport2Xls.Columns.Add("<b>Reporting Manager</b>");
            mdtExport2Xls.Columns.Add("<b>Consultant Name</b>");
            mdtExport2Xls.Columns.Add("<b>Pending for Approval (Hours)</b>");
            mdtExport2Xls.Columns.Add("<b>Approved (Hours)</b>");
            mdtExport2Xls.Columns.Add("<b>Total Hours</b>");

            DataRow drAddItem;
            Decimal ldec = 0;
            foreach (ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();
                drAddItem[0] = ((Label)lvi.FindControl("ReportingManager")).Text;   // Consultant name
                drAddItem[1] = ((Label)lvi.FindControl("UserCode")).Text;   // Consultant name
                ldec = CS2Dec(((Label)lvi.FindControl("PendingHours")).Text);       // Pending for Approval (Hours)
                drAddItem[2] = ldec.ToString();

                ldec = CS2Dec(((Label)lvi.FindControl("ApprovedHours")).Text);       // Approved (Hours)
                drAddItem[3] = ldec.ToString();

                ldec = CS2Dec(((Label)lvi.FindControl("TotalHours")).Text);       // Total Hours
                drAddItem[4] = ldec.ToString();

                //if (!((Label)lvi.FindControl("UserCode")).Visible)
                //{
                //    drAddItem[2] = "<b>Grand Total:</b>";
                //    drAddItem[3] = "<b>" + ldec.ToString() + "</b>";
                //    drAddItem[7] = string.Empty;
                //}
                mdtExport2Xls.Rows.Add(drAddItem);
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

        #region DataValidate
        protected bool DateValidate(string adate)
        {
            bool valid = true;
            try
            {
                CultureInfo ivC = new CultureInfo("es-US");
                DateTime.ParseExact(adate, "MM/dd/yyyy", ivC);
            }
            catch (Exception ex)
            {
                valid = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + "The '" + adate + "' date format is not supported in calendar. Please entry the date format dd/MM/yyyy or selection the day in the popup calendar." + "','');", true);
            }
            return valid;
        }
        #endregion

        private void LoadDBName()
        {
            dsDBName = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBList");
            ddl_Company.DataSource = dsDBName.Tables[0];
            ddl_Company.DataTextField = "DBName";
            ddl_Company.DataValueField = "DBName";
            ddl_Company.DataBind();
            ddl_Company.Items.Insert(0, "");
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            DateValidate(((TextBox)sender).Text.Trim());
        }

        #endregion
    }
}