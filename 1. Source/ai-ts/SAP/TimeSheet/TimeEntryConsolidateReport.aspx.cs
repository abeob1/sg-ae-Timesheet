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
    public partial class TimeEntryConsolidateReport : System.Web.UI.Page
    {
        private static bool mb_Export2Excel = false;
        private static DataSet ds = new DataSet();
        private static DataTable mdtExport2Xls = new DataTable();
        private static string mMonth = DateTime.Now.Month.ToString();
        private static string mYear = DateTime.Now.Year.ToString();

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
            mMonth = ddl_Month.SelectedValue;
            mYear = ddl_Year.SelectedValue;
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
        }

        #endregion

        #region Methods

        #region Binding
        protected void Binding()
        {
            
            ddl_Month.Items.FindByValue(mMonth).Selected = true;
            ddl_Year.Items.FindByValue(mYear).Selected = true;
            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_TimeEntryConsolidateReport",
                Data.CreateParameter("@IN_Month", ddl_Month.SelectedValue),
                Data.CreateParameter("@IN_Year", ddl_Year.SelectedValue));

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
                if (!ds.Tables[0].Rows[i]["ReportToName"].ToString().Equals(""))
                {
                    //int idx = lvi.Controls.IndexOf((Label)lvi.FindControl("Date"));
                    //lvi.Controls.Remove((Label)lvi.FindControl("Date"));
                    ((Label)lvi.FindControl("Hours")).Font.Bold = true;
                    ((Label)lvi.FindControl("Manager")).Font.Bold = true;
                    //Label lblTotal = new Label();
                    //lblTotal.ID = "lblTotal" + i.ToString();
                    //if (i == lvStage.Items.Count - 1)
                    //{
                    //    lblTotal.Text = "Grand Total:";
                    //    lblTotal.Font.Bold = true;
                    //    lvi.Controls.AddAt(1, lblTotal);
                    //    //((Label)lvi.FindControl("PrjCode")).Text = string.Empty;
                    //}
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
            mdtExport2Xls.Columns.Add("<b>Manager</b>");
            mdtExport2Xls.Columns.Add("<b>Consultant Name</b>");
            mdtExport2Xls.Columns.Add("<b>Hours</b>");

            DataRow drAddItem;
            Decimal ldec = 0;
            foreach (ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();
                drAddItem[0] = ((Label)lvi.FindControl("Manager")).Text;   // Manager
                drAddItem[1] = ((Label)lvi.FindControl("UserCode")).Text;   // Consultant name
                ldec = CS2Dec(((Label)lvi.FindControl("Hours")).Text);       // Hour
                drAddItem[2] = ldec.ToString();
                if (((Label)lvi.FindControl("Manager")).Font.Bold)
                {
                    drAddItem[0] = "<b>"+ ((Label)lvi.FindControl("Manager")).Text + "</b>";
                    drAddItem[2] = "<b>" + ldec.ToString() + "</b>";
                    
                }
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

        #endregion
    }
}