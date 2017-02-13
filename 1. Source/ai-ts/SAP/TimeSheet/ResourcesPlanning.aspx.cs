using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SAP.WebServices;
using System.Collections;
using System.Globalization;
using SAP.Admin.DAO;
using SAP.Admin;

namespace SAP
{
    public partial class ResourcesPlanning : System.Web.UI.Page
    {

        #region WeeklyURL
        protected string WeeklyURL
        {
            get { return ViewState["WeeklyURL"].ToString(); }
            set { ViewState["WeeklyURL"] = value; }
        }
        #endregion

        #region dsPrjList
        protected DataSet dsPrjList
        {
            get { return (DataSet)ViewState["dsPrjList"]; }
            set { ViewState["dsPrjList"] = value; }
        }
        #endregion

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

        #region DaysOfWeek
        protected DateTime Monday
        {
            get { return (DateTime)ViewState["Monday"]; }
            set { ViewState["Monday"] = value; }        
        }

        protected DateTime Tuesday
        {
            get { return (DateTime)ViewState["Tuesday"]; }
            set { ViewState["Tuesday"] = value; }
        }

        protected DateTime Wednesday
        {
            get { return (DateTime)ViewState["Wednesday"]; }
            set { ViewState["Wednesday"] = value; }
        }

        protected DateTime Thursday
        {
            get { return (DateTime)ViewState["Thursday"]; }
            set { ViewState["Thursday"] = value; }
        }

        protected DateTime Friday
        {
            get { return (DateTime)ViewState["Friday"]; }
            set { ViewState["Friday"] = value; }
        }

        protected DateTime Saturday
        {
            get { return (DateTime)ViewState["Saturday"]; }
            set { ViewState["Saturday"] = value; }
        }

        protected DateTime Sunday
        {
            get { return (DateTime)ViewState["Sunday"]; }
            set { ViewState["Sunday"] = value; }
        }
        #endregion

        #region ds
        protected DataSet ds
        {
            get { return (DataSet)ViewState["ds"]; }
            set { ViewState["ds"] = value; }
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

        #region Events

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ds = new DataSet();                
                if (Request.QueryString["fromdate"] != null && Request.QueryString["fromdate"].Length == 10
                    && Request.QueryString["todate"] != null && Request.QueryString["todate"].Length == 10)
                {
                    txtFromDate .Text = FromDate = Request.QueryString["fromdate"];
                    txtToDate.Text = ToDate = Request.QueryString["todate"];
                }
                else
                {
                    BuildPeriod();
                }

                BuiltDataSet();
                Binding(); 
            }
        }
        #endregion

        #region OnLoadComplete
        protected override void OnLoadComplete(EventArgs e)
        {
            try
            {
                base.OnLoadComplete(e);
                if (this.Request["__EVENTARGUMENT"] != null && this.Request["__EVENTARGUMENT"].ToString() != "")
                {
                    switch (this.Request["__EVENTARGUMENT"].ToString())
                    {
                        case "MultiProjectCallBack":
                            break;

                        case "EditProjectCallBack":
                            break;

                        default:
                            break;
                    }
                    
                }
                // set day name of week in header
                //UpdateListViewHeader();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            }

            txtFromDate.Text = FromDate;
        }
        #endregion

        #region btnFilter Click
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FromDate = txtFromDate.Text.Trim();
            ToDate = txtToDate.Text.Trim();
            Response.Redirect("/TimeSheet/ResourcesPlanning.aspx?fromdate=" + txtFromDate.Text.Trim() + "&todate=" + txtToDate.Text.Trim());
        }
        #endregion

        #region UploadButton Click
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (this.FileUploadControl.PostedFile != null && this.FileUploadControl.PostedFile.FileName.Length > 0)
            {
                this.FileStreamObj = this.FileUploadControl.PostedFile.InputStream;
                this.ReadFile();
                UpdateResourcesPlanning();
            }
            else
            {
                // Do nothing.
            }
        }
        #endregion

        #endregion

        #region Methods

        #region BuildPeriod
        protected void BuildPeriod()
        {
            int liMonth = DateTime.Now.Month;
            if (liMonth > 9)
            {
                txtFromDate.Text = FromDate = "09/01/" + DateTime.Now.Year.ToString();
                txtToDate.Text = ToDate = "08/31/" + (DateTime.Now.Year + 1).ToString();
            }
            else
            {
                txtFromDate.Text = FromDate = "09/01/" + (DateTime.Now.Year - 1).ToString();
                txtToDate.Text = ToDate = "08/31/" + DateTime.Now.Year.ToString();
            }
        }
        #endregion

        protected void BuiltDataSet()
        {
            CultureInfo ivC = new CultureInfo("es-US");
            DateTime fromdate = Convert.ToDateTime(txtFromDate.Text, ivC);
            DateTime todate = Convert.ToDateTime(txtToDate.Text, ivC);

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetResourcePlanning",
                Data.CreateParameter("@IN_BeginDate", fromdate.ToString("MM/dd/yyyy")),
                Data.CreateParameter("@IN_EndDate", todate.ToString("MM/dd/yyyy")));
            /*if (this.FileLines != null)
            {
                DataTable dt = new DataTable("ResourcesPlanning");
                // Create columns header
                string[] header = this.FileLines[0].ToString().Split(',');
                for (int i = 0; i < header.Length; i++)
                {
                    dt.Columns.Add(new DataColumn(header[i], Type.GetType("System.String")));
                }
                // Create DataRows
                for (int x = 1; x < this.FileLines.Count; x++)
                {
                    DataRow row = dt.NewRow();
                    string[] rowData = this.FileLines[x].ToString().Split(',');
                    for (int iCol = 0; iCol < rowData.Length; iCol++)
                    {
                        row[header[iCol]] = rowData[iCol];
                    }

                    dt.Rows.Add(row);
                }
                ds = new DataSet();
                ds.Tables.Add(dt);
            }*/
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

        #region BuiltTable
        protected void BuiltTable()
        {
            // add all the table header
            TableRow HeaderRow = new TableRow();
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Columns.Count; x++) //
                {
                    HeaderRow.Cells.Add(new TableCell());
                    //if (x == 0 || x == 1)
                    //{
                    //    HeaderRow.Cells[x].CssClass = "GridViewHeader";
                    //    HeaderRow.Cells[x].Text = ds.Tables[0].Columns[x].ToString();
                    //}
                    //else
                    //{
                    //    HeaderRow.Cells[x].CssClass = "GridViewHeader";
                    //    HeaderRow.Cells[x].Text = ds.Tables[0].Columns[x].ToString();
                    //}
                    HeaderRow.Cells[x].CssClass = "GridViewHeader";
                    HeaderRow.Cells[x].Text = ds.Tables[0].Columns[x].ToString();
                    HeaderRow.Cells[x].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            else
            {
                HeaderRow.Cells.Add(new TableCell());
                HeaderRow.Cells[0].CssClass = "GridViewHeader";
                HeaderRow.Cells[0].Text = "No Data";
                HeaderRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            this.Table_ResourcesPlanningGrid.Rows.Add(HeaderRow);
        }
        #endregion

        #region BuiltRow
        protected void BuiltRow()
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int idxRow = 0; idxRow < ds.Tables[0].Rows.Count; idxRow++)
                {
                    // add all the table row
                    TableRow DataRow = new TableRow();
                    for (int x = 0; x < ds.Tables[0].Columns.Count; x++) //
                    {
                        DataRow.Cells.Add(new TableCell());
                        DataRow.Cells[x].CssClass = "GridViewItem";
                        DataRow.Cells[x].Text = (ds.Tables[0].Rows[idxRow][x].ToString().Replace(".00", "") == "0") ? "" : ds.Tables[0].Rows[idxRow][x].ToString().Replace(".00", "");
                        if (x != 0 && x != 1) DataRow.Cells[x].HorizontalAlign = HorizontalAlign.Center;
                    }
                    this.Table_ResourcesPlanningGrid.Rows.Add(DataRow);
                }
            }
            this.Table_ResourcesPlanningGrid.BorderWidth = 1;
            this.Table_ResourcesPlanningGrid.BorderColor = System.Drawing.Color.Gray;
        }
        #endregion

        #region Binding
        void Binding()
        {
            if (this.Table_ResourcesPlanningGrid.Rows.Count > 0) this.Table_ResourcesPlanningGrid.Rows.Clear();
            this.BuiltTable();
            this.BuiltRow();
        }
        #endregion

        #region CS2Dec
        private decimal CS2Dec(string asValue)
        {
            try
            {
                return decimal.Parse(asValue);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region UpdateListViewHeader
        protected void UpdateListViewHeader()
        {
            
        }
        #endregion

        protected bool UpdateResourcesPlanning()
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
                    // 
                    string[] header = this.FileLines[0].ToString().Split(',');

                    // Delete data before import new
                    lfromdate = Convert.ToDateTime(header[2], ivC);
                    ltodate = Convert.ToDateTime(header[header.Length - 1], ivC);

                    KeyParameters.Add(new DatabaseParameter("CONVERT(VARCHAR(8), WeekStart, 112)", lfromdate.ToString("yyyyMMdd"), DBDataType.String, DBLinkage.AND, DBCompareType.LargerNEqual));
                    KeyParameters.Add(new DatabaseParameter("CONVERT(VARCHAR(8), WeekStart, 112)", ltodate.ToString("yyyyMMdd"), DBDataType.String, DBLinkage.AND, DBCompareType.SmallerNEqual));
                    // delete before insert
                    SqlHelper.DeleteCommand(KeyParameters, "tbl_ResouresPlanning");
                    sqlList.Add(SqlHelper.SQL);


                    for (int col = 2; col < header.Length; col++)
                    {
                        for (int x = 1; x < this.FileLines.Count; x++)
                        {
                            string[] rowData = FileLines[x].ToString().Split(',');
                            KeyParameters.Clear();
                            KeyParameters.Add(new DatabaseParameter("UserCode", rowData[0]));
                            KeyParameters.Add(new DatabaseParameter("PrjCode", rowData[1]));
                            KeyParameters.Add(new DatabaseParameter("PrjName", ""));
                            KeyParameters.Add(new DatabaseParameter("WeekStart", header[col]));
                            KeyParameters.Add(new DatabaseParameter("Hours", rowData[col]));
                            KeyParameters.Add(new DatabaseParameter("UserImported", User.Identity.Name));

                            SqlHelper.InsertCommand(KeyParameters, "tbl_ResouresPlanning");
                            sqlList.Add(SqlHelper.SQL);
                        }
                    }
                }

                if (!(smooth = SqlHelper.ExecuteQuery(sqlList, Data.ConnectionString)))
                {
                    StatusLabel.Text = SqlHelper.ErrMsg;
                }
                else
                {
                    Response.Redirect("/TimeSheet/ResourcesPlanning.aspx?fromdate=" + lfromdate.ToString("MM/dd/yyyy") + "&todate=" + ltodate.ToString("MM/dd/yyyy"));
                    StatusLabel.Text = "Import Resource Planning Successful.";
                }
            }

            return smooth;
        }

        private List<string> GetUserList()
        {
            List<string> UserList = new List<string>();
            MembershipUserCollection Users =  Membership.GetAllUsers();
            foreach (MembershipUser user in Users)
            {
                if (!UserList.Contains(user.UserName))
                {
                    UserList.Add(user.UserName);
                }
            }
            return UserList;
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

        protected bool DataValidation()
        {
            bool valid = true;
            string lsUsers = string.Empty, lsProject = string.Empty;
            List<string> UserList = new List<string>();
            List<string> ProjectList = new List<string>();

            UserList = GetUserList();
            ProjectList = GetProjectList();

            for (int x = 1; x < this.FileLines.Count; x++)
            {
                string[] rowData = FileLines[x].ToString().Split(',');
                
                if(!UserList.Contains(rowData[0]))
                {
                    lsUsers = (string.IsNullOrEmpty(lsUsers) ? rowData[0] : lsUsers + "; " + rowData[0]);
                    valid = false;
                }
                if (!ProjectList.Contains(rowData[1]))
                {
                    lsProject = (string.IsNullOrEmpty(lsProject) ? rowData[1] : lsProject + "; " + rowData[1]);
                    valid = false;
                }
            }

            if (!valid)
            {
                StatusLabel.ForeColor = System.Drawing.Color.Red;
                StatusLabel.Font.Bold = true;
                string lsMsg = "Import Status: fail. Users invalid: " + lsUsers + ". Projects invalid: " + lsProject;
                StatusLabel.Text = "Import Status: fail. <br/> Users invalid: " + lsUsers + ". <br/> Projects invalid: " + lsProject; ;
                Session["errorMessage"] = lsMsg;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + GeneralFunctions.UrlFullEncode(lsMsg) + "','');", true);
            }
            return valid;
        }

        #endregion
    }
}