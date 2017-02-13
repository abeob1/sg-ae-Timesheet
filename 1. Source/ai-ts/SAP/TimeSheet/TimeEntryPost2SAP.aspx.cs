using System;
using System.Text;
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
using System.IO;
using SAP.Admin.DAO;
using System.Drawing;
namespace SAP
{
    public partial class TimeEntryPost2SAP : System.Web.UI.Page
    {
        private static GeneralFunctions GF = null;//new GeneralFunctions(HttpContext.Current.User.Identity.Name);
        private static DataSet ds;
        private static DataSet dsSummary = new DataSet();
        private static string FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        private static string ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");

        private static DataTable dtJEHeader = new DataTable();
        private static DataTable dtJELine = new DataTable();

        private static DataTable dtJVHeader = new DataTable();
        private static DataTable dtJVLine = new DataTable();
        private static DataTable dtJVLineCost = new DataTable();
        private static DataTable dtProjectLIst;
        private static DataTable mdtExport2Xls = new DataTable();
        private static bool mb_Export2Excel = false;
        private static string msCompany = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Binding();
            }
        }

        void Binding()
        {
            LoadDBName();

            txtFromDate.Text = FromDate;
            txtToDate.Text = ToDate;
            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ConsultantList",
                Data.CreateParameter("@IN_BeginDate", txtFromDate.Text),
                Data.CreateParameter("@IN_EndDate", txtToDate.Text),
                Data.CreateParameter("@IN_SAPB1DB", ddlCompany.SelectedValue)
                );
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();

           
            this.lvSimulate.Items.Clear();
            this.lvSimulate.DataBind();
            this.lvSimulateAcc.Items.Clear();
            this.lvSimulateAcc.DataBind();

            dsSummary = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ConsultantListSummary",
               Data.CreateParameter("@IN_BeginDate", txtFromDate.Text),
               Data.CreateParameter("@IN_EndDate", txtToDate.Text),
               Data.CreateParameter("@IN_SAPB1DB", ddlCompany.SelectedValue)
               );
            DataView dvSummary = new DataView(dsSummary.Tables[0]);

            this.lvSummary.DataSource = dvSummary;
            this.lvSummary.DataBind();

            generateJV();
            if (dtJVLine.Rows.Count > 0 || dtJVLineCost.Rows.Count > 0)
                btnPost.Enabled = true;
        }

        protected void Import_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (mb_Export2Excel) ExportToExcelWithFormat();
        }


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

        protected void Filter_Click(object sender, EventArgs e)
        {
            btnPost.Enabled = false;
            FromDate = txtFromDate.Text.Trim();
            ToDate = txtToDate.Text.Trim();
            msCompany = ddlCompany.SelectedValue;
            Binding();
            lbError.Text = "";
        }

        private void updateAllPosted() {

            SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_UpdateTimeEntryPosted", Data.CreateParameter("@IN_BeginDate", txtFromDate.Text),
            Data.CreateParameter("@IN_EndDate", txtToDate.Text),
            Data.CreateParameter("@IN_SAPB1DB", ddlCompany.SelectedValue)
            );
        }

        private bool postJVForLabourAct()
        {

            DocumentXML objInfo = new DocumentXML();
            DataSet dsJV;

            DataTable tbCompany = dtJVLine.DefaultView.ToTable(true, "Company");

            Array arrContentsCols = new string[] { "Debit", "Credit" };

            DataTable eachCompany;
            eachCompany = new DataTable("BTF1");

            eachCompany = dtJVLine.Clone();
            SAP.WebServices.Transaction ts = new WebServices.Transaction();
            ts.Timeout = 1000000;
            foreach (DataRow row in tbCompany.Rows)
            {
                String companyName = "";
                companyName = row["Company"].ToString();

                dsJV = new DataSet("DS");
                dsJV.Tables.Add(dtJVHeader.Copy());
                DataRow[] drows;
                eachCompany.Rows.Clear();

                drows = dtJVLine.Select("Company = '" + companyName + "' AND (Debit <> 0 OR Credit <> 0)");

                if (drows.Length > 0)
                {
                    //string newact = "";
                    foreach (DataRow rw in drows)
                    {
                        DataRow dr = eachCompany.NewRow();
                        string act = rw["Account"].ToString();
                        //newact;
                        //newact =  act.Substring(0, 5) + "-" + act.Substring(5, 2) + "-" + act.Substring(7, 2);
                        dr["Company"] = rw["Company"];
                        dr["Account"] = act;
                        dr["Debit"] = rw["Debit"];
                        dr["Credit"] = rw["Credit"];
                        dr["Project"] = rw["Project"];
                        eachCompany.Rows.Add(dr);

                    }
                    //dsJV.Tables.Add(eachCompany);

                    dsJV.Tables.Add(GF.ResetFormatNumeric(eachCompany.Copy(), arrContentsCols));


                    String xmlStrJV = "";

                    xmlStrJV = objInfo.ToXMLStringFromDS("28", dsJV);

                    //2. post to SAP (JV)

                    DataSet dsResult = ts.CreateMarketingDocument(xmlStrJV, companyName, "28", "", false);//User.Identity.Name
                    if ((int)dsResult.Tables[0].Rows[0]["ErrCode"] != 0)
                    {
                        //this.lbError.Text = dsResult.Tables[0].Rows[0]["ErrMsg"].ToString();
                        Session["errorMessage"] = dsResult.Tables[0].Rows[0]["ErrMsg"];
                        Session["requestXML"] = xmlStrJV;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                            "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(dsResult.Tables[0].Rows[0]["ErrMsg"].ToString() + "-" + xmlStrJV) + "','');", true);
                        return false;
                    }

                }

            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            return true;

        }

        private bool postJVForBillingAct()
        {
            
            DocumentXML objInfo = new DocumentXML();
            DataSet dsJV;
            DataTable tbCompany = dtJVLineCost.DefaultView.ToTable(true, "Company");//true: distinct
            Array arrContentsCols = new string[] { "Debit", "Credit" };

            DataTable eachCompany;
            eachCompany = new DataTable("BTF1");

            eachCompany = dtJVLineCost.Clone();
            SAP.WebServices.Transaction ts = new WebServices.Transaction();
            ts.Timeout = 1000000;
            
            foreach (DataRow row in tbCompany.Rows)
            {
                String companyName = row["Company"].ToString();
                dsJV = new DataSet("DS");
                dsJV.Tables.Add(dtJVHeader.Copy());
                DataRow[] drows;
                eachCompany.Rows.Clear();
                drows = dtJVLineCost.Select("Company = '" + companyName + "' AND (Debit <> 0 OR Credit <> 0)");
                if (drows.Length > 0)
                {
                    //string newact = "";
                    foreach (DataRow rw in drows)
                    {
                        DataRow dr = eachCompany.NewRow();
                        dr["Company"] = rw["Company"];

                        string act = rw["Account"].ToString();
                        //newact = act.Substring(0, 5) + "-" + act.Substring(5, 2) + "-" + act.Substring(7, 2);
// "_SYS00000000189";// newact;
                        dr["Account"] = act;
                        dr["Debit"] = rw["Debit"];
                        dr["Credit"] = rw["Credit"];
                        dr["Project"] = rw["Project"];
                        eachCompany.Rows.Add(dr);
                    }
                    
                    //dsJV.Tables.Add(eachCompany.Copy());
                    dsJV.Tables.Add(GF.ResetFormatNumeric(eachCompany.Copy(), arrContentsCols));
                    String xmlStrJV = objInfo.ToXMLStringFromDS("28", dsJV);

                    //2. post to SAP (JV)
                    
                    DataSet dsResult = ts.CreateMarketingDocument(xmlStrJV, companyName, "28", "", false);//User.Identity.Name
                    if ((int)dsResult.Tables[0].Rows[0]["ErrCode"] != 0)
                    {
                        this.lbError.Text = dsResult.Tables[0].Rows[0]["ErrMsg"].ToString();
                        Session["errorMessage"] = dsResult.Tables[0].Rows[0]["ErrMsg"];
                        Session["requestXML"] = xmlStrJV;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                            "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(dsResult.Tables[0].Rows[0]["ErrMsg"].ToString() + "-" + xmlStrJV) + "','');", true);
                        return false;
                    }
                    
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            return true;
        }
        private Boolean checkInvalidProject()
        {
            foreach (DataRow dr in dtProjectLIst.Rows)
            {
                if (dr["Active"].ToString() == "N")
                {

                    Session["errorMessage"] = "Project Code: " + dr["ProjectCodeOnly"].ToString() + " is inactive.";

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                        "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(dr["ProjectCodeOnly"].ToString() + " is inactive.") + "','');", true);
                    return false;
                }
            }
            return true;
        }

        protected void Post_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkInvalidProject())
                {
                    if (GF == null) 
                           GF = new GeneralFunctions(HttpContext.Current.User.Identity.Name); // Added by thangnv
                    if (postJVForLabourAct() && postJVForBillingAct())
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                        //update status.
                        updateAllPosted();
                        Filter_Click(null, null);
                        this.btnPost.Enabled = false; //cannot post again

                        this.lbError.Text = "Operation complete sucessful!";
                        Session["successMessage"] = "Operation complete sucessful!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                           "Main.setMasterMessage('" + "Operation complete sucessful!" + "','');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                }
            }
            catch (Exception ex)
            {
                this.lbError.Text = ex.Message;
                Session["errorMessage"] = ex.Message;
                
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                    "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(ex.Message) + "','');", true);
                
            }
        }

        private void createTablesJV() {

            
            dtJVHeader = new DataTable("OBTF");
            dtJVHeader.Columns.Add("RefDate");
            dtJVHeader.Columns.Add("DueDate");
            dtJVHeader.Columns.Add("TaxDate");
            dtJVHeader.Columns.Add("Memo");
            dtJVHeader.Columns.Add("Ref1");
            dtJVHeader.Columns.Add("Ref2");
            dtJVHeader.Columns.Add("Ref3");
            dtJVHeader.Columns.Add("U_UserID");


            dtJVLine = new DataTable("BTF1");
            DataColumn col;
            col = new DataColumn("No");
            col.DefaultValue = "xx_remove_xx";
            dtJVLine.Columns.Add(col);

            dtJVLine.Columns.Add("Account");
            dtJVLine.Columns.Add("AccountName");

            col = new DataColumn("Company");
            col.DefaultValue = "xx_remove_xx";
            dtJVLine.Columns.Add(col);           

            dtJVLine.Columns.Add("ShortName");

            col = new DataColumn("Dscription");
            col.DefaultValue = "xx_remove_xx";
            dtJVLine.Columns.Add(col);

            DataColumn colDeb = new DataColumn("Debit");
            colDeb.DataType = System.Type.GetType("System.Double");
            dtJVLine.Columns.Add(colDeb);

            DataColumn colCre = new DataColumn("Credit");
            colCre.DataType = System.Type.GetType("System.Double");
            dtJVLine.Columns.Add(colCre);

            dtJVLine.Columns.Add("Project");

            dtJVLineCost = dtJVLine.Copy();
        }

        private void generateJV()
        {
            createTablesJV();
            CultureInfo ivC = new System.Globalization.CultureInfo("es-US");

            DataRow drJV = dtJVHeader.NewRow();
            drJV["RefDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text, ivC));
            drJV["DueDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text, ivC));
            drJV["TaxDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text, ivC));
            drJV["Memo"] = "Posted from timesheet";
            drJV["Ref1"] = "Posted from timesheet";
            drJV["Ref2"] = "Posted from timesheet";
            drJV["Ref3"] = "Posted from timesheet";
            drJV["U_UserID"] = User.Identity.Name;
            dtJVHeader.Rows.Add(drJV);

            if (dsSummary != null)
            {
                // get from SBOWEB
                dtProjectLIst = GetprojectList(); //GF.ObjectSelect("sp_ProjectList", new ArrayList(), null);

                foreach (DataRow row in dsSummary.Tables[0].Rows)
                {
                    string company = row["Company"].ToString();
                    string prjCode = row["PrjCode"].ToString();

                    DataRow[] rowsOfProjectList = dtProjectLIst.Select("Company = '" + company + "' AND ProjectCodeOnly = '" + prjCode + "'");
                    if (rowsOfProjectList.Length > 0)
                    {
                        string cogsAcc = rowsOfProjectList[0]["U_COGSAct"].ToString();
                        string laborAcc = rowsOfProjectList[0]["U_LaborAct"].ToString();
                        string billingAct = rowsOfProjectList[0]["U_PrjBillingAct"].ToString();
                        string salesAct = rowsOfProjectList[0]["U_PrjSalesAct"].ToString();

                        string cogsAccName = rowsOfProjectList[0]["U_COGSActName"].ToString();
                        string laborAccName = rowsOfProjectList[0]["U_LaborActName"].ToString();
                        string billingActName = rowsOfProjectList[0]["U_PrjBillingActName"].ToString();
                        string salesActName = rowsOfProjectList[0]["U_PrjSalesActName"].ToString();

                        double amount = Double.Parse(row["Amount"].ToString());

                        double accumulate = Double.Parse(rowsOfProjectList[0]["Accumulate"].ToString() == "" ? "0" : rowsOfProjectList[0]["Accumulate"].ToString());

                        if (amount != 0)
                        {
                            if (cogsAcc != "" && laborAcc != "")
                            {
                                DataRow dr = dtJVLine.NewRow();
                                dr["Company"] = company;
                                dr["Account"] = cogsAcc;
                                dr["AccountName"] = cogsAccName;
                                dr["Debit"] = amount;
                                dr["Credit"] = 0;
                                dr["Project"] = row["PrjCode"];
                                dtJVLine.Rows.Add(dr);

                                dr = dtJVLine.NewRow();
                                dr["Company"] = company;
                                dr["Account"] = laborAcc; //72200-01-00
                                dr["AccountName"] = laborAccName;

                                dr["Debit"] = 0;
                                dr["Credit"] = amount;
                                dr["Project"] = row["PrjCode"];
                                dtJVLine.Rows.Add(dr);

                            }

                           double prjCost = Double.Parse(rowsOfProjectList[0]["U_AI_ProjCost"].ToString());
                           double tgtCost = Double.Parse(rowsOfProjectList[0]["U_AI_TargetCost"].ToString());

                            if (tgtCost != 0 && prjCost != 0)
                            {
                                double recRevenueAmt = (amount / tgtCost) * prjCost;
                                if (billingAct != "" && salesAct != "")//Recognize Revenue
                                {

                                    if (accumulate + recRevenueAmt <= tgtCost)
                                    {
                                        DataRow dr = dtJVLineCost.NewRow();
                                        dr["Company"] = company;
                                        dr["Account"] = billingAct; //72200-01-00        
                                        dr["AccountName"] = billingActName;

                                        dr["Debit"] = Double.Parse(recRevenueAmt.ToString("0.##"));

                                        dr["Credit"] = 0;
                                        dr["Project"] = row["PrjCode"];

                                        dtJVLineCost.Rows.Add(dr);

                                        dr = dtJVLineCost.NewRow();
                                        dr["Company"] = company;
                                        dr["Account"] = salesAct; //72200-01-00
                                        dr["AccountName"] = salesActName;

                                        dr["Debit"] = 0;

                                        dr["Credit"] = Double.Parse(recRevenueAmt.ToString("0.##"));

                                        dr["Project"] = row["PrjCode"];
                                        dtJVLineCost.Rows.Add(dr);
                                    }
                                    else {
                                        double subAmt = tgtCost - accumulate;
                                        DataRow dr = dtJVLineCost.NewRow();
                                        dr["Company"] = company;
                                        dr["Account"] = billingAct; //72200-01-00        
                                        dr["AccountName"] = billingActName;

                                        dr["Debit"] = Double.Parse(subAmt.ToString("0.##"));

                                        dr["Credit"] = 0;
                                        dr["Project"] = row["PrjCode"];

                                        dtJVLineCost.Rows.Add(dr);

                                        dr = dtJVLineCost.NewRow();
                                        dr["Company"] = company;
                                        dr["Account"] = salesAct; //72200-01-00
                                        dr["AccountName"] = salesActName;

                                        dr["Debit"] = 0;

                                        dr["Credit"] = Double.Parse(subAmt.ToString("0.##"));

                                        dr["Project"] = row["PrjCode"];
                                        dtJVLineCost.Rows.Add(dr);
                                    }
                                }

                            }
                        }
                    }
                }
                lvSimulate.DataSource = dtJVLine;
                lvSimulate.DataBind();
                lvSimulateAcc.DataSource = dtJVLineCost;
                lvSimulateAcc.DataBind();
            }
            else
            {
                lvSimulate.DataSource = null;
                lvSimulate.DataBind();
                lvSimulateAcc.DataSource = null;
                lvSimulateAcc.DataBind();
            }
        }

        protected void createTablesJE()
        {
            dtJEHeader = new DataTable("OJDT");
            dtJEHeader.Columns.Add("RefDate");
            dtJEHeader.Columns.Add("DueDate");
            dtJEHeader.Columns.Add("TaxDate");
            dtJEHeader.Columns.Add("Memo");
            dtJEHeader.Columns.Add("Ref1");
            dtJEHeader.Columns.Add("Ref2");
            dtJEHeader.Columns.Add("Ref3");
            dtJEHeader.Columns.Add("U_UserID");

            dtJELine = new DataTable("JDT1");
            DataColumn col = new DataColumn("No");
            col.DefaultValue = "xx_remove_xx";
            dtJELine.Columns.Add(col);

            dtJELine.Columns.Add("Account");

            col = new DataColumn("Dscription");
            col.DefaultValue = "xx_remove_xx";
            dtJELine.Columns.Add(col);

            dtJELine.Columns.Add("Debit");
            dtJELine.Columns.Add("Credit");

            col = new DataColumn("Project");
            col.DefaultValue = "xx_remove_xx";
            dtJELine.Columns.Add(col);

            
        }

        protected void GenerateJE()
        {
            createTablesJE();
            DataRow drJE = dtJEHeader.NewRow();
            drJE["RefDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text));
            drJE["DueDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text));
            drJE["TaxDate"] = String.Format("{0:yyyyMMdd}", DateTime.Parse(txtToDate.Text));
            drJE["Memo"] = "Posted from timesheet";
            drJE["Ref1"] = "Posted from timesheet";
            drJE["Ref2"] = "Posted from timesheet";
            drJE["Ref3"] = "Posted from timesheet";
            drJE["U_UserID"] = User.Identity.Name;
            dtJEHeader.Rows.Add(drJE);

            if (dsSummary != null) {
                foreach (DataRow row in dsSummary.Tables[0].Rows)
                {
                    DataRow dr = dtJELine.NewRow();
                    dr["Account"] = "103000"; //21820-00-00
                    dr["Debit"] = row["Amount"];
                    dr["Credit"] = 0;
                    dr["Project"] = row["PrjCode"];
                    dtJELine.Rows.Add(dr);

                    dr = dtJELine.NewRow();
                    dr["Account"] = "103500"; //72200-01-00
                    dr["Debit"] = 0;
                    dr["Credit"] = row["Amount"];
                   dr["Project"] = row["PrjCode"];
                    dtJELine.Rows.Add(dr);

                    lvSimulate.DataSource = dtJELine;
                    lvSimulate.DataBind();
                }
            }
        }

        public void ExportToSpreadsheet(DataTable table, string name)
        {
            //HttpContext context = HttpContext.Current;
            Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                Response.Write(column.ColumnName + ";");
            }
            Response.Write(Environment.NewLine);
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    Response.Write(row[i].ToString().Replace(";", string.Empty) + ";");
                }
                Response.Write(Environment.NewLine);
            }
            Response.ContentType = "text/csv";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
            Response.End();
        }

        
        protected void btnExport_Click(object sender, EventArgs e)
        {

            mb_Export2Excel = true;
            BuuildDT();
            Response.Redirect(Request.RawUrl);
        }

        private void ExportToExcel()
        {
            
            mb_Export2Excel = false;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=TimeEntryPost2SAP.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            DataGrid dg = new DataGrid();
            dg.DataSource = ds.Tables[0];
            dg.DataBind();
            for (int i = 0; i < dg.Items.Count; i++)
            {
                //Apply text style to each Row
                dg.Items[i].Attributes.Add("class", "textmode");
            }

            dg.RenderControl(hw);

            /*DataGrid dgSum = new DataGrid();
            dgSum.DataSource = dsSummary.Tables[0];
            dgSum.DataBind();
            dgSum.RenderControl(hw);*/
            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
             
        }

        private void ExportToExcel(string fileName)
        {
            
            mb_Export2Excel = false;
            Response.ContentType = "application/csv";
            Response.Charset = "";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            DataTable dtb = ds.Tables[0];
            try
            {
                StringBuilder sb = new StringBuilder();
                //Add Header        
                int[] icolumn = new int[] { 1, 2, 3, 4, 5 };
                for (int count = 0; count < icolumn.Length; count++)
                {
                    if (dtb.Columns[icolumn[count]].ColumnName != null)
                        sb.Append(dtb.Columns[icolumn[count]].ColumnName);
                    if (count < icolumn.Length - 1)
                    {
                        sb.Append("\t");
                    }
                }
                Response.Write(sb.ToString() + "\n");
                Response.Flush();
                //Append Data        
                int soDem = 0;
                while (dtb.Rows.Count >= soDem + 1)
                {
                    sb = new StringBuilder();
                    for (int col = 0; col < icolumn.Length - 1; col++)
                    {
                        if (dtb.Rows[soDem][icolumn[col]] != null)
                            sb.Append(dtb.Rows[soDem][icolumn[col]].ToString().Replace(",", " "));
                        sb.Append("\t");
                    }
                    if (dtb.Rows[soDem][dtb.Columns.Count - 1] != null)
                        sb.Append(dtb.Rows[soDem][dtb.Columns.Count - 1].ToString().Replace(",", " "));
                    Response.Write(sb.ToString() + "\n");
                    Response.Flush();
                    soDem = soDem + 1;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            dtb.Dispose();
            Response.End();
             
        }

        private void ExportToExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = "DownloadMobileNoExcel.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }

        protected void UploadDataTableToExcel(DataTable dtRecords)
        {
            string XlsPath = Server.MapPath(@"~/Add_data/test.xls");
            string attachment = string.Empty;
            if (XlsPath.IndexOf("\\") != -1)
            {
                string[] strFileName = XlsPath.Split(new char[] { '\\' });
                attachment = "attachment; filename=" + strFileName[strFileName.Length - 1];
            }
            else
                attachment = "attachment; filename=" + XlsPath;
            try
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = string.Empty;

                foreach (DataColumn datacol in dtRecords.Columns)
                {
                    Response.Write(tab + datacol.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");

                foreach (DataRow dr in dtRecords.Rows)
                {
                    tab = "";
                    for (int j = 0; j < dtRecords.Columns.Count; j++)
                    {
                        Response.Write(tab + Convert.ToString(dr[j]));
                        tab = "\t";
                    }

                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
        }

        private DataTable GetprojectList()
        {
            DataSet dsPrjList = null;
            try
            {
                dsPrjList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
                if (dsPrjList == null || dsPrjList.Tables == null) return null;
            }
            catch (Exception ex)
            {
                Session["errorMessage"] = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(ex.Message) + "','');", true);
                return null;
            }

            return dsPrjList.Tables[0];
        }

        private void ExportToExcelWithFormat()
        {
            //Temp Grid
            DataGrid dg = new DataGrid();
            dg.DataSource = mdtExport2Xls;
            dg.DataBind();
            ExportToExcel("TimeEntryPost2SAP.xls", dg);
            dg = null;
            dg.Dispose();
        }

        private void BuuildDT()
        {
            
            //Create Tempory Table
            mdtExport2Xls = new DataTable();
            //Creating Header Row
            mdtExport2Xls.Columns.Add("<b>#</b>");
            mdtExport2Xls.Columns.Add("<b>Project Code</b>");
            mdtExport2Xls.Columns.Add("<b>Project Name</b>");
            mdtExport2Xls.Columns.Add("<b>Date</b>");
            mdtExport2Xls.Columns.Add("<b>Consultant</b>");
            mdtExport2Xls.Columns.Add("<b>Hours</b>");
            mdtExport2Xls.Columns.Add("<b>Rate</b>");
            mdtExport2Xls.Columns.Add("<b>Amount</b>");

            DataRow drAddItem;
            Decimal ldec = 0;
            int li_rc = ds.Tables[0].Rows.Count;
            /*
            foreach (DataRow dr in ds.Tables[0].Rows) {
                drAddItem = mdtExport2Xls.NewRow();

                drAddItem[0] = "";

                drAddItem[1] = dr["PrjCode"];//Project Code
                drAddItem[2] = dr["PrjName"];//Project Code
                drAddItem[3] = dr["Date"]; // Date
                drAddItem[4] = dr["U_UserID"]; // Consultant
                ldec = CS2Dec(dr["Hour"].ToString()); // Hour
                drAddItem[5] = ldec.ToString();
                drAddItem[6] = dr["u_ai_ratehour"]; // Rate
                ldec = CS2Dec(dr["Amount"].ToString()); // Amount
                drAddItem[7] = ldec.ToString();

                mdtExport2Xls.Rows.Add(drAddItem);
            }
            */
            foreach(ListViewDataItem lvi in lvStage.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();

                drAddItem[0] = 1;// (((CheckBox)lvi.FindControl("Import")).Checked ? 1 : 0);//#
                
                drAddItem[1] = ((Label)lvi.FindControl("PrjCode")).Text;//Project Code
                drAddItem[2] = ((Label)lvi.FindControl("PrjName")).Text;//Project Code
                drAddItem[3] = ((Label)lvi.FindControl("Date")).Text; // Date
                drAddItem[4] = ((Label)lvi.FindControl("Consultant")).Text; // Consultant
                ldec = CS2Dec(((Label)lvi.FindControl("Hour")).Text); // Hour
                drAddItem[5] = ldec.ToString();
                drAddItem[6] = ((Label)lvi.FindControl("Rate")).Text; // Rate
                ldec = CS2Dec(((Label)lvi.FindControl("Amount")).Text); // Amount
                drAddItem[7] = ldec.ToString();

                mdtExport2Xls.Rows.Add(drAddItem);
            }

            // Summary Data ////////////////////////
            /*drAddItem = mdtExport2Xls.NewRow();
            mdtExport2Xls.Rows.Add(drAddItem);
            drAddItem = mdtExport2Xls.NewRow();
            drAddItem[1] = "<b>Summary</b>";
            mdtExport2Xls.Rows.Add(drAddItem);
            drAddItem = mdtExport2Xls.NewRow();
            drAddItem[1] = "<b>Company</b>";
            drAddItem[2] = "<b>Project Code</b>";
            drAddItem[3] = "<b>Amount</b>";
            mdtExport2Xls.Rows.Add(drAddItem);
            foreach (ListViewDataItem lvi in lvSummary.Items)
            {
                drAddItem = mdtExport2Xls.NewRow();

                drAddItem[1] = ((Label)lvi.FindControl("Company")).Text;
                drAddItem[2] = ((Label)lvi.FindControl("ProjCode")).Text;
                drAddItem[3] = ((Label)lvi.FindControl("Amount")).Text;
              
                mdtExport2Xls.Rows.Add(drAddItem);
            }*/
            
        }

        private void ExportToExcel(string strFileName, DataGrid dg)
        {
            mb_Export2Excel = false;
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }


        public bool ThumbnailCallback()
        {
            return false;
        }

        private void LoadDBName()
        {
            DataSet dsDBName = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBList");
            ddlCompany.DataSource = dsDBName.Tables[0];
            ddlCompany.DataTextField = "DBName";
            ddlCompany.DataValueField = "DBName";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, "");
            ddlCompany.Items.FindByValue(msCompany).Selected = true;
        }

    }
}