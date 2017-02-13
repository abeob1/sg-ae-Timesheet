using System;
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
    public partial class TimeMyEntryReport : System.Web.UI.Page
    {
        private static bool mb_Export2Excel = false;
        private static DataTable mdtExport2Xls = new DataTable();
        private static DataSet ds = new DataSet();
        private static DataSet dsProject = new DataSet();
        private static string FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        private static string ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Binding();
            }
        }

        void Binding()
        {
            txtFromDate.Text = FromDate;
            txtToDate.Text = ToDate;

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_TimeMyEntryReport",
                Data.CreateParameter("@IN_BeginDate", txtFromDate.Text), 
                Data.CreateParameter("@IN_EndDate", txtToDate.Text), 
                Data.CreateParameter("@IN_UserCode", User.Identity.Name));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FromDate = txtFromDate.Text.Trim();
            ToDate = txtToDate.Text.Trim();
            Binding();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            SetForm();
            if (mb_Export2Excel) ExportToExcelWithFormat();
        }

        private void SetForm()
        {
            for(int i = 0; i < lvStage.Items.Count ; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                if (ds.Tables[0].Rows[i]["PrjCode"].ToString().Equals("ZZZZZZZZZZZZZZZZZZZZ"))
                {
                    // lvi.Controls.Remove((LinkButton)lvi.FindControl("imgbEdit"));
                    // lvi.Controls.Remove((LinkButton)lvi.FindControl("imgbDelete"));
                    int idx = lvi.Controls.IndexOf((Label)lvi.FindControl("Date"));
                    lvi.Controls.Remove((Label)lvi.FindControl("Date"));
                    ((Label)lvi.FindControl("Hour")).Font.Bold = true;
                    Label lblTotal = new Label();
                    lblTotal.ID = "lblTotal"+ i.ToString();
                    if (i == lvStage.Items.Count - 1)
                    {
                        ((Label)lvi.FindControl("UserCode")).Text = string.Empty;
                        lblTotal.Text = "Grand Total:";
                        lblTotal.Font.Bold = true;
                        lvi.Controls.AddAt(idx, lblTotal);
                        ((Label)lvi.FindControl("PrjCode")).Text = string.Empty;
                    }
                }
            }
        }

        protected void btnExport2Xlsx_Click(object sender, EventArgs e)
        {
            mb_Export2Excel = true;
            BuuildDT();
            Response.Redirect(Request.RawUrl);
        }

        #region ExportToExcelWithFormat
        private void ExportToExcelWithFormat()
        {
            DataGrid dg = new DataGrid();
            dg.DataSource = mdtExport2Xls;
            dg.DataBind();
            FillDataToExcel("My_Entry_4_approval.xls", dg);
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
            mdtExport2Xls.Columns.Add("<b>Consultant Name</b>");
            mdtExport2Xls.Columns.Add("<b>Date</b>");
            mdtExport2Xls.Columns.Add("<b>Hour</b>");
            mdtExport2Xls.Columns.Add("<b>Project Code</b>");
            mdtExport2Xls.Columns.Add("<b>Project Name</b>");
            mdtExport2Xls.Columns.Add("<b>Task</b>");
            mdtExport2Xls.Columns.Add("<b>Comments</b>");

            DataRow drAddItem;
            Decimal ldec = 0;
            int li = 0;
            foreach (ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();
                drAddItem[0] = ((Label)lvi.FindControl("UserCode")).Text;   // Consultant Name
                drAddItem[1] = ((Label)lvi.FindControl("Date")).Text;       // Date
                ldec = CS2Dec(((Label)lvi.FindControl("Hour")).Text);       // Hour
                drAddItem[2] = ldec.ToString();
                
                drAddItem[3] = ((Label)lvi.FindControl("PrjCode")).Text;    // Project Code
                drAddItem[4] = ((Label)lvi.FindControl("PrjName")).Text;    // Project Name
                drAddItem[5] = ((Label)lvi.FindControl("Task")).Text;       // Task
                drAddItem[6] = ((Label)lvi.FindControl("Comments")).Text;   // Comments

                if (((Label)lvi.FindControl("PrjName")).Text.Equals(""))
                {
                    if (li == lvStage.Items.Count - 1)
                    {
                        drAddItem[0] = string.Empty;
                        drAddItem[1] = "<b>" + "Grand Total:" + "</b>";
                        drAddItem[2] = "<b>" + ldec.ToString() + "</b>";
                    }
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
    }
}