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
    public partial class TimeEntryReport : System.Web.UI.Page
    {
        private static bool mb_Export2Excel = false;
        private static DataSet ds = new DataSet();
        private static DataTable mdtExport2Xls = new DataTable();
        private static string FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        private static string ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");

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
                UserList chosenItem = null;
                Project chosenPrj = null;
                switch (this.Request["__EVENTARGUMENT"].ToString())
                {
                    case "ReportingManagerCallBack":
                        chosenItem = Session["chosenUser"] as UserList;
                        itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                        if (chosenItem != null)
                        {
                            txtReportTo.Text = chosenItem.UserName;
                        }
                        break;

                    case "ConsultantFilterCallBack":
                        chosenItem = Session["chosenUser"] as UserList;
                        itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                        if (chosenItem != null)
                        {
                            txtConsultant.Text = chosenItem.UserName;
                        }
                        break;

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
            txtFromDate.Text = FromDate;
            txtToDate.Text = ToDate;

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_TimeEntryReport",
                Data.CreateParameter("@IN_BeginDate", txtFromDate.Text),
                Data.CreateParameter("@IN_EndDate", txtToDate.Text),
                Data.CreateParameter("@IN_ReportToName", txtReportTo.Text.Trim()),
                Data.CreateParameter("@IN_Consultant", txtConsultant.Text.Trim()),
                Data.CreateParameter("@IN_ProjectCode", txtProjectCode.Text.Trim()));

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

                if (ds.Tables[0].Rows[i]["PrjCode"].ToString().Equals("ZZZZZZZZZZZZZZZZZZZZ") || ds.Tables[0].Rows[i]["ID"].ToString().Equals("0"))
                {
                    //((Label)lvi.FindControl("Manager")).Font.Bold = true;

                    int idx = lvi.Controls.IndexOf((Label)lvi.FindControl("Date"));
                    lvi.Controls.Remove((Label)lvi.FindControl("Date"));

                    ((Label)lvi.FindControl("Hour")).Font.Bold = true;
                    ((Label)lvi.FindControl("rate_hour")).Font.Bold = true;
                    ((Label)lvi.FindControl("TotalCost")).Font.Bold = true;

                    Label lblTotal = new Label();
                    lblTotal.ID = "lblTotal" + i.ToString();

                    if (i == lvStage.Items.Count - 1)
                    {
                        lblTotal.Text = "Grand Total:";
                        lblTotal.Font.Bold = true;
                        lvi.Controls.AddAt(idx, lblTotal);
                        ((Label)lvi.FindControl("PrjCode")).Text =
                        ((Label)lvi.FindControl("ReportingManager")).Text =
                        ((Label)lvi.FindControl("UserCode")).Text =
                        ((Label)lvi.FindControl("PrjCode")).Text = string.Empty;
                    }
                    //else
                    //{
                    //    lblTotal.Text = "Total:";
                    //    lblTotal.Font.Bold = true;
                    //    lvi.Controls.AddAt(idx, lblTotal);
                    //}
                }
                //else
                //{
                //    ((Label)lvi.FindControl("Manager")).Text = string.Empty;
                //}
            }
        }
        #endregion

        #region ExportToExcelWithFormat
        private void ExportToExcelWithFormat()
        {
            DataGrid dg = new DataGrid();
            dg.DataSource = mdtExport2Xls;
            dg.DataBind();
            FillDataToExcel("TimeEntryReport.xls", dg);
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
            mdtExport2Xls.Columns.Add("<b>Consultant</b>");
            mdtExport2Xls.Columns.Add("<b>Project Type</b>");
            mdtExport2Xls.Columns.Add("<b>Project Manager</b>");
            mdtExport2Xls.Columns.Add("<b>Project Code</b>");
            mdtExport2Xls.Columns.Add("<b>Project Name</b>");
            mdtExport2Xls.Columns.Add("<b>Customer</b>");
            mdtExport2Xls.Columns.Add("<b>Task</b>");
            mdtExport2Xls.Columns.Add("<b>Comments</b>");
            mdtExport2Xls.Columns.Add("<b>Date</b>");
            mdtExport2Xls.Columns.Add("<b>Hours</b>");
            mdtExport2Xls.Columns.Add("<b>Rate per Hour</b>");
            mdtExport2Xls.Columns.Add("<b>Total Cost</b>");
            mdtExport2Xls.Columns.Add("<b>Status</b>");

            DataRow drAddItem;
            Decimal ldec = 0, ldecRateHour = 0, ldecTotalCost = 0;
            int li = 0;
            foreach (ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();
                drAddItem[0] = ((Label)lvi.FindControl("ReportingManager")).Text;    // Reporting Manager
                drAddItem[1] = ((Label)lvi.FindControl("UserCode")).Text;   // Consultant
                drAddItem[2] = ((Label)lvi.FindControl("PrjType")).Text;    // Project Type
                drAddItem[3] = ((Label)lvi.FindControl("PrjManager")).Text; // Project Manager
                drAddItem[4] = ((Label)lvi.FindControl("PrjCode")).Text;    // Project Code
                drAddItem[5] = ((Label)lvi.FindControl("PrjName")).Text;    // Project Name
                drAddItem[6] = ((Label)lvi.FindControl("CustName")).Text;    // Customer
                drAddItem[7] = ((Label)lvi.FindControl("Task")).Text;       // Task
                drAddItem[8] = ((Label)lvi.FindControl("Comments")).Text;   // Comments
                drAddItem[9] = ((Label)lvi.FindControl("Date")).Text;       // Date
                ldec = CS2Dec(((Label)lvi.FindControl("Hour")).Text);       // Hour
                drAddItem[10] = ldec.ToString();
                //11. rate_hour
                ldecRateHour = CS2Dec(((Label)lvi.FindControl("rate_hour")).Text);       // rate_hour
                drAddItem[11] = ldecRateHour.ToString();
                //12. TotalCost
                ldecTotalCost = CS2Dec(((Label)lvi.FindControl("TotalCost")).Text);       // rate_hour
                drAddItem[12] = ldecTotalCost.ToString();

                drAddItem[13] = ((Label)lvi.FindControl("Status")).Text;     // Status name
                if (((Label)lvi.FindControl("UserCode")).Text.Equals(""))
                {
                    //drAddItem[0] = "<b>" + ((Label)lvi.FindControl("ReportingManager")).Text + "</b>";
                    if(li == lvStage.Items.Count -1)
                        drAddItem[8] = "<b>" + "Grand Total:" + "</b>";
                    //else drAddItem[8] = "<b>" + "Total:" + "</b>";
                    drAddItem[10] = "<b>" + ldec.ToString() + "</b>";
                    drAddItem[11] = "<b>" + ldecRateHour.ToString() + "</b>";
                    drAddItem[12] = "<b>" + ldecTotalCost.ToString() + "</b>";
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

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            DateValidate(((TextBox)sender).Text.Trim());
        }

        #endregion

    }
}