using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
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
    public partial class DirectExpenseEntry : System.Web.UI.Page
    {
        private static DataSet ds = new DataSet();

        #region FromDate
        protected string FromDate
        {
            get { return ViewState["FromDate"].ToString(); }
            set { ViewState["FromDate"] = value; }
        }
        #endregion

        #region ToDate
        protected string ToDate
        {
            get { return ViewState["ToDate"].ToString(); }
            set { ViewState["ToDate"] = value; }
        }
        #endregion

        #region FileStreamObj
        protected Stream FileStreamObj
        {
            get { return (Stream)Session["FileStreamObj"]; }
            set { Session["FileStreamObj"] = value; }
        }
        #endregion

        #region FileLines
        protected ArrayList FileLines
        {
            get { return (ArrayList)ViewState["FileLines"]; }
            set { ViewState["FileLines"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["fromdate"] != null && Request.QueryString["fromdate"].Length == 10
                    && Request.QueryString["todate"] != null && Request.QueryString["todate"].Length == 10)
                {
                    txtFromDate.Text = FromDate = Request.QueryString["fromdate"];
                    txtToDate.Text = ToDate = Request.QueryString["todate"];
                }
                else
                {
                    txtFromDate.Text = FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                    txtToDate.Text = ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");
                }
                Binding();
            }
        }

        void Binding()
        {
            CultureInfo ivC = new CultureInfo("es-US");
            DateTime BeginDate = Convert.ToDateTime(txtFromDate.Text, ivC);
            DateTime EndDate = Convert.ToDateTime(txtToDate.Text, ivC);

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_DirectExpenseEntry", Data.CreateParameter("@IN_BeginDate", BeginDate.ToString("MM/dd/yyyy")),
                Data.CreateParameter("@IN_EndDate", EndDate.ToString("MM/dd/yyyy") ));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            switch (e.CommandName)
            {
                case "Update":
                    Save(lvi);
                    break;

                case "Edit":
                    btnAddRecord.Enabled = false;
                    break;

                case "Delete":
                    int li_ErrorCode = 0;
                    try
                    {
                        string lsID = dr["internal_id"].ToString();
                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Delete tbl_DirectExpense Where internal_id = '" + lsID + "'");
                    }
                    catch (SqlException sqlEx)
                    {
                        li_ErrorCode = sqlEx.ErrorCode; 
                    }

                    if (li_ErrorCode != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + li_ErrorCode.ToString() + "','');", true);
                    }
                    else
                    {
                        Session["successMessage"] = "Operation complete sucessful!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + "Operation complete sucessful!" + "','');", true);
                        Response.Redirect(Request.RawUrl);
                    } break;

                case "Cancel":
                    Response.Redirect(Request.RawUrl);
                    break;
            }
        }

        protected void lvStage_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            this.lvStage.EditIndex = e.NewEditIndex;
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            try
            {
                base.OnLoadComplete(e);
                if (this.Request["__EVENTARGUMENT"] != null && this.Request["__EVENTARGUMENT"].ToString() != "")
                {
                    Int32 itemNo = 0;
                    DataView dv = new DataView(ds.Tables[0]);
                    switch (this.Request["__EVENTARGUMENT"].ToString())
                    {
                        case "EditProjectCallBack":
                            Project chosenItem = Session["chosenProject"] as Project;
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            if (chosenItem != null)
                            {
                                AddUpdateItem(itemNo, chosenItem);
                                this.lvStage.DataSource = dv;
                                this.lvStage.EditIndex = ds.Tables[0].Rows.Count - 1;
                                this.lvStage.DataBind();
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            }
        }

        protected void lvStage_ItemCreated(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvStage_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvStage.EditIndex >= 0)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                if (dataItem.DisplayIndex == lvStage.EditIndex)
                {
                   

                }
            }
        }

        protected void lvStage_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void lvStage_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lvStage_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        private void Save( ListViewItem lvi)
        {
            try
            {
                string lsID = ((Label)lvi.FindControl("lblinternal_idEdit")).Text;
                string lsDate = ((TextBox)lvi.FindControl("txtDateEdit")).Text;
                string lsPrjCode = ((Label)lvi.FindControl("lblPrjCodeEdit")).Text;
                string lsAmount = ((TextBox)lvi.FindControl("txtAmountEdit")).Text;
                string lsSAPB1DB = ((Label)lvi.FindControl("lblSAPB1DBEdit")).Text;

                if (string.IsNullOrEmpty(lsPrjCode) || string.IsNullOrEmpty(lsDate)) 
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('The Date or Project cannot be empty.','');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                    return;
                }
                SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_InsUpdateDirectExpense",
                    Data.CreateParameter("@IN_InternalID", lsID),
                    Data.CreateParameter("@IN_Date", DateTime.Parse(lsDate)),
                    Data.CreateParameter("@IN_PrjCode", lsPrjCode),
                    Data.CreateParameter("@IN_Amount", decimal.Parse(lsAmount)),
                    Data.CreateParameter("@IN_SAPB1DB", lsSAPB1DB));

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            }
        }

        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Binding();
            Response.Redirect("/TimeSheet/DirectExpenseEntry.aspx?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text);
        }

        #region AddUpdateItem
        private void AddUpdateItem(Int32 itemNo, Project chosenItem)
        {
            if (chosenItem != null)
            {
                Int32 CurNo = 0;

                DataRow drX;
                if (itemNo == 0)
                {
                    drX = ds.Tables[0].NewRow();
                    CurNo = ds.Tables[0].Rows.Count + 1;
                    drX["internal_id"] = string.Empty;
                    //drX["No"] = CurNo;
                    CurNo = ds.Tables[0].Rows.Count + 1;
                }
                else
                {
                    drX = ds.Tables[0].Rows[itemNo - 1];
                    CurNo = itemNo;
                }

                string[] lsArr = chosenItem.PrjCode.Split(';');
                //int iNo = ds.Tables[0].Rows.Count + 1;

                drX["No"] = CurNo.ToString();
                drX["Date"] = DateTime.Now.ToString("MM/dd/yyyy");
                drX["PrjCode"] = lsArr[0];
                drX["Amount"] = 0;
                drX["SAPB1DB"] = lsArr[1];
               
                if (itemNo == 0) ds.Tables[0].Rows.Add(drX);
            }
        }
        #endregion

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (this.FileUploadControl.PostedFile != null && this.FileUploadControl.PostedFile.FileName.Length > 0)
            {
                this.FileStreamObj = this.FileUploadControl.PostedFile.InputStream;
                this.ReadFile();
                UpdateDirectExpense();
            }
            else
            {
                // Do nothing.
            }
        }

        #region ReadFile
        public virtual bool ReadFile()
        {
            bool smooth = true;
            try
            {
                using (StreamReader Reader = new StreamReader(this.FileStreamObj, System.Text.UTF8Encoding.UTF8))
                {
                    if (FileLines == null) FileLines = new ArrayList();
                    else FileLines.Clear();
                    string line;
                    // Read and display lines from the file until the end of the file is reached.
                    while ((line = Reader.ReadLine()) != null)
                    {
                        FileLines.Add(line);
                    }
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                smooth = false;
            }
            return smooth;
        }
        #endregion

        protected bool UpdateDirectExpense()
        {
            bool smooth = true;
            if (DataValidation())
            {
                ArrayList sqlList = new ArrayList();
                DatabaseParameters KeyParameters = new DatabaseParameters();
                CultureInfo ivC = new CultureInfo("es-US");
                DateTime lfromdate = Convert.ToDateTime(txtFromDate.Text, ivC);
                DateTime ltodate = Convert.ToDateTime(txtToDate.Text, ivC);

                // now insert new
                if (this.FileLines != null)
                {
                    // Delete data before import new
                    lfromdate = Convert.ToDateTime(txtFromDate.Text, ivC);
                    ltodate = Convert.ToDateTime(txtToDate.Text, ivC);

                    KeyParameters.Add(new DatabaseParameter("CONVERT(VARCHAR(8), [Date], 112)", lfromdate.ToString("yyyyMMdd"), DBDataType.String, DBLinkage.AND, DBCompareType.LargerNEqual));
                    KeyParameters.Add(new DatabaseParameter("CONVERT(VARCHAR(8), [Date], 112)", ltodate.ToString("yyyyMMdd"), DBDataType.String, DBLinkage.AND, DBCompareType.SmallerNEqual));
                    // delete before insert
                    SqlHelper.DeleteCommand(KeyParameters, "tbl_DirectExpense");
                    sqlList.Add(SqlHelper.SQL);

                    for (int x = 1; x < this.FileLines.Count; x++)
                    {
                        string[] rowData = FileLines[x].ToString().Split(',');
                        KeyParameters.Clear();
                        string lsID = Guid.NewGuid().ToString();
                        KeyParameters.Add(new DatabaseParameter("internal_id", lsID ));
                        KeyParameters.Add(new DatabaseParameter("PrjCode", rowData[0]));
                        KeyParameters.Add(new DatabaseParameter("Date", Convert.ToDateTime(rowData[1] , ivC) ));
                        KeyParameters.Add(new DatabaseParameter("Amount", rowData[2] ));
                        KeyParameters.Add(new DatabaseParameter("SAPB1DB", "UserImported"));

                        SqlHelper.InsertCommand(KeyParameters, "tbl_DirectExpense");
                        sqlList.Add(SqlHelper.SQL);
                    }
                }

                if (!(smooth = SqlHelper.ExecuteQuery(sqlList, Data.ConnectionString)))
                {
                    StatusLabel.Text = SqlHelper.ErrMsg;
                }
                else
                {
                    Response.Redirect("/TimeSheet/DirectExpenseEntry.aspx?fromdate=" + lfromdate.ToString("MM/dd/yyyy") + "&todate=" + ltodate.ToString("MM/dd/yyyy"));
                    Binding();
                    StatusLabel.Text = "Import Direct Expense Entry Successful.";
                }
            }

            return smooth;
        }

        protected bool DataValidation()
        {
            bool valid = true;
            string lsProject = string.Empty;
            List<string> ProjectList = new List<string>();

            ProjectList = GetProjectList();

            for (int x = 1; x < this.FileLines.Count; x++)
            {
                string[] rowData = FileLines[x].ToString().Split(',');

                if (!ProjectList.Contains(rowData[0]))
                {
                    lsProject = (string.IsNullOrEmpty(lsProject) ? rowData[0] : lsProject + "; " + rowData[0]);
                    valid = false;
                }
            }

            if (!valid)
            {
                StatusLabel.ForeColor = System.Drawing.Color.Red;
                StatusLabel.Font.Bold = true;
                string lsMsg = "Import failure. Projects invalid: " + lsProject;
                StatusLabel.Text = "Import failure. <br/> Projects invalid: " + lsProject; ;
                Session["errorMessage"] = lsMsg;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(lsMsg) + "','');", true);
            }
            return valid;
        }

        private List<string> GetProjectList()
        {
            List<string> ProjectCodes = new List<string>();
            DataSet dsPrjList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");

            foreach (DataRow row in dsPrjList.Tables[0].Rows)
            {
                string lsPrj = row["PrjCode"].ToString().Split(';')[0];
                if (!ProjectCodes.Contains(lsPrj))
                {
                    ProjectCodes.Add(lsPrj);
                }
            }

            return ProjectCodes;
        }

    }
}