using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAP.WebServices;
using System.Collections;
using System.Globalization;
using System.Net;
using Microsoft.Reporting.WebForms;
using System.IO;
using SAP.Admin.DAO;

namespace SAP
{
    public partial class SummaryOfTimesheetDailyReport : System.Web.UI.Page
    {
        public static string ReportFullFileName = "~/Reports/rptSummaryofTimesheetDaily.rdlc";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "") return;
                CultureInfo ivC = new System.Globalization.CultureInfo("es-US");

                objReportViewer.Reset();
                objReportViewer.ProcessingMode = ProcessingMode.Local;
                objReportViewer.LocalReport.ReportPath = Server.MapPath(ReportFullFileName);
                
                DataSet ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_SummaryOfTimesheetDailyViewReport",
                    Data.CreateParameter("@IN_BeginDate", Convert.ToDateTime(txtFromDate.Text, ivC)),
                    Data.CreateParameter("@IN_EndDate", Convert.ToDateTime(txtToDate.Text, ivC))
                );

                

                ReportDataSource datasource = new ReportDataSource("dsSummaryofTimesheetDaily", ds.Tables[0]);
                objReportViewer.LocalReport.DataSources.Clear();
                objReportViewer.LocalReport.DataSources.Add(datasource);
                objReportViewer.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);            
            }
        }
    }
}