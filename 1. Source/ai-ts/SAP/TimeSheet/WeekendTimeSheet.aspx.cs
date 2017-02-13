using System;
using System.Collections.Generic;
using System.Text;
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
using SAP.Admin;

namespace SAP
{
    public partial class WeekendTimeSheet : System.Web.UI.Page
    {
        private static string msPhase = string.Empty;

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

        //#region ds
        //protected DataSet ds
        //{
        //    get { return (DataSet)ViewState["ds"]; }
        //    set { ViewState["ds"] = value; }
        //}
        //#endregion

        #region ds
        protected int PeriodStatus
        {
            get { return (int)ViewState["PeriodStatus"]; }
            set { ViewState["PeriodStatus"] = value; }
        }
        #endregion

        #region Event 

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PeriodStatus = 0;

                if (Request.QueryString["date"] != null && Request.QueryString["date"].Length == 10)
                    FromDate = Request.QueryString["date"];
                else
                    FromDate = DateTime.Now.ToString("MM/dd/yyyy");

                Binding();
                string opt = Request.QueryString["opt"];
                if (opt == "1")
                {
                    btnSubmit.Visible = true;
                    btnAddRecord.Visible = false;
                    WeeklyURL = "/TimeSheet/WeekendTimeSheet.aspx?opt=1";
                }
                else
                {
                    btnSubmit.Visible = false;
                    btnAddRecord.Visible = true;
                    WeeklyURL = "/TimeSheet/WeekendTimeSheet.aspx?opt=0";
                }
                GeneratePeriodStatus();
                lblMessage.Visible = false;
            }
            
        }
        #endregion

        #region lvStage_ItemCommand
        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            switch (e.CommandName)
            {
                case "Update":
                    if (DataValidation(e.Item.DataItemIndex, lvi))
                    {
                       Save(((Label)lvi.FindControl("lblIDSEdit")).Text.Split(';'), lvi);
                       hdnCountry.Value = hdnEBMName.Value = hdnProspectName.Value = string.Empty;
                       hdnStatus.Value = "View";
                    }
                    break;

                case "Edit":
                    btnAddRecord.Enabled = false;
                    txtFromDate.Enabled = false;
                    msPhase = ((Label)lvi.FindControl("lblPhase")).Text;
                    string[] lsMoreInfo = ((Label)lvi.FindControl("lblMoreInfo")).Text.Split(';');
                    if (lsMoreInfo != null)
                    {
                        if (lsMoreInfo.Length > 0) hdnCountry.Value = lsMoreInfo[0];
                        if (lsMoreInfo.Length > 1) hdnEBMName.Value = lsMoreInfo[1];
                        if (lsMoreInfo.Length > 2) hdnProspectName.Value = lsMoreInfo[2];
                        hdnStatus.Value = "Editing";
                    }
                    break;

                case "Delete":
                    if (PeriodStatus == 1) return;
                    int li_ErrorCode = 0;
                    try
                    {
                        string lsIDSin = dr["IDS"].ToString().Replace(';', ',');

                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Update tbl_TimeEntry Set [Status] = -1 Where ID in ( " + lsIDSin + " )");
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
                        //FromDate = txtFromDate.Text.Trim();
                        Response.Redirect(WeeklyURL + "&date=" + txtFromDate.Text.Trim());
                    } break;

                case "Cancel":
                    //FromDate = txtFromDate.Text.Trim();
                    Response.Redirect(WeeklyURL + "&date="+ txtFromDate.Text.Trim());
                    break;
            }
        }
        #endregion

        #region lvStage_ItemEditing
        protected void lvStage_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            if (PeriodStatus == 1) 
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Timesheet Posting Period had locked.');", true);
                e.Cancel = true;
                return;
            }
            DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
            this.lvStage.EditIndex = e.NewEditIndex;
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }
        #endregion

        #region lvStage_ItemDataBound
        protected void lvStage_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvStage.EditIndex >= 0)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                if (dataItem.DisplayIndex == lvStage.EditIndex)
                {
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlPrjTaskEdit"); if (ddl != null)
                    {
                        string lsPrjCode = ((Label)e.Item.FindControl("lblPrjCodeEdit")).Text;
                        string lsSAPB1DB = ((Label)e.Item.FindControl("lblSAPB1DBEdit")).Text;
                        ddl.DataSource = dsPrjList.Tables[0];
                        ddl.DataTextField = "PrjName";
                        ddl.DataValueField = "PrjCode";
                        ddl.DataBind();
                        if (!lsPrjCode.Equals("") && !lsSAPB1DB.Equals(""))
                            ddl.SelectedValue = lsPrjCode + ";" + lsSAPB1DB;
                    }

                    DropDownList ddlPhase = (DropDownList)e.Item.FindControl("ddlPhaseEdit"); if (ddlPhase != null)
                    {
                        if (ddlPhase.Items.FindByValue(msPhase) != null)
                        {
                            ddlPhase.Items.FindByValue(msPhase).Selected = true;
                            msPhase = string.Empty;
                        }
                        else
                            ddlPhase.SelectedIndex = 0;
                    }

                    TextBox txtMon = (TextBox)e.Item.FindControl("txtMonEdit");
                    TextBox txtTue = (TextBox)e.Item.FindControl("txtTueEdit");
                    TextBox txtWed = (TextBox)e.Item.FindControl("txtWedEdit");
                    TextBox txtThu = (TextBox)e.Item.FindControl("txtThuEdit");
                    TextBox txtFri = (TextBox)e.Item.FindControl("txtFriEdit");
                    TextBox txtSat = (TextBox)e.Item.FindControl("txtSatEdit");
                    TextBox txtSun = (TextBox)e.Item.FindControl("txtSunEdit");
                    TextBox txtSrvType = (TextBox)e.Item.FindControl("txtSrvTypeEdit");
                    TextBox txtTimeEndDate = (TextBox)e.Item.FindControl("txtTimeEndDateEdit");

                    txtMon.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtMon.ClientID + "', '" + txtSrvType.Text + "', '" + Monday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtTue.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtTue.ClientID + "', '" + txtSrvType.Text + "', '" + Tuesday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtWed.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtWed.ClientID + "', '" + txtSrvType.Text + "', '" + Wednesday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtThu.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtThu.ClientID + "', '" + txtSrvType.Text + "', '" + Thursday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtFri.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtFri.ClientID + "', '" + txtSrvType.Text + "', '" + Friday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtSat.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtSat.ClientID + "', '" + txtSrvType.Text + "', '" + Saturday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                    txtSun.Attributes.Add("onKeyUp", "txtOnKeyPress( '" + txtSun.ClientID + "', '" + txtSrvType.Text + "', '" + Sunday.ToString("yyyyMMdd") + "', '" + txtTimeEndDate.Text + "' )");
                }
                //mbVisible = true;
                System.Web.UI.HtmlControls.HtmlTableCell td = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdButtons");
                if (td != null) td.Visible = true;
            }
            else
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    System.Web.UI.HtmlControls.HtmlTableCell td = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdButtons");
                    if (td != null) td.Visible = btnAddRecord.Enabled;
                    
                }
            }
        }
        #endregion

        #region lvStage_LayoutCreated
        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {
            // calculate Grand Total
            decimal ldecMonTotal = 0, ldecTueTotal = 0, ldecWedTotal = 0, ldecThuTotal = 0, ldecFriTotal = 0, ldecSatTotal = 0, ldecSunTotal = 0, ldecAllTotal = 0;
            DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ldecMonTotal += CS2Dec(dr["Mon"].ToString());
                ldecTueTotal += CS2Dec(dr["Tue"].ToString());
                ldecWedTotal += CS2Dec(dr["Wed"].ToString());
                ldecThuTotal += CS2Dec(dr["Thu"].ToString());
                ldecFriTotal += CS2Dec(dr["Fri"].ToString());
                ldecSatTotal += CS2Dec(dr["Sat"].ToString());
                ldecSunTotal += CS2Dec(dr["Sun"].ToString());
                ldecAllTotal += CS2Dec(dr["Total"].ToString());
            }

            Label lblMonTotal = (Label)lvStage.FindControl("lblMonTotal");
            if (lblMonTotal != null) lblMonTotal.Text = ldecMonTotal.ToString();

            Label lblTueTotal = (Label)lvStage.FindControl("lblTueTotal");
            if (lblTueTotal != null) lblTueTotal.Text = ldecTueTotal.ToString();

            Label lblWedTotal = (Label)lvStage.FindControl("lblWedTotal");
            if (lblWedTotal != null) lblWedTotal.Text = ldecWedTotal.ToString();

            Label lblThuTotal = (Label)lvStage.FindControl("lblThuTotal");
            if (lblThuTotal != null) lblThuTotal.Text = ldecThuTotal.ToString();

            Label lblFriTotal = (Label)lvStage.FindControl("lblFriTotal");
            if (lblFriTotal != null) lblFriTotal.Text = ldecFriTotal.ToString();

            Label lblSatTotal = (Label)lvStage.FindControl("lblSatTotal");
            if (lblSatTotal != null) lblSatTotal.Text = ldecSatTotal.ToString();

            Label lblSunTotal = (Label)lvStage.FindControl("lblSunTotal");
            if (lblSunTotal != null) lblSunTotal.Text = ldecSunTotal.ToString();

            Label lblAllTotal = (Label)lvStage.FindControl("lblAllTotal");
            if (lblAllTotal != null) lblAllTotal.Text = ldecAllTotal.ToString();
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
                    DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
                    Int32 itemNo = 0;
                    DataView dv = new DataView(ds.Tables[0]);
                    switch (this.Request["__EVENTARGUMENT"].ToString())
                    {
                        case "MultiProjectCallBack":
                            ArrayList chosenItems = Session["chosenProject"] as ArrayList;
                            foreach (Project item in chosenItems)
                            {
                                AddUpdateItem(0, item);
                            }

                            this.lvStage.DataSource = dv;
                            this.lvStage.DataBind();
                            break;

                        case "EditProjectCallBack":
                            Project chosenItem = Session["chosenProject"] as Project;
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            if (chosenItem != null)
                            {
                                string[] prjArr = chosenItem.PrjCode.ToString().Split(';');
                                if (CheckBelongCompany(prjArr[0], prjArr[1]))
                                {
                                    AddUpdateItem(itemNo, chosenItem);

                                    this.lvStage.DataSource = dv;
                                    this.lvStage.EditIndex = ds.Tables[0].Rows.Count - 1;
                                    this.lvStage.DataBind();
                                }
                                hdnStatus.Value = "Adding";
                            }
                            break;

                        case "MoreInfoCallBack":
                            if (Session["chosenMoreInfo"] != null)
                            {
                                string lsMoreInfo = Session["chosenMoreInfo"] as string;
                                //itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                                if (string.IsNullOrEmpty(lsMoreInfo))
                                    hdnCountry.Value = hdnEBMName.Value = hdnProspectName.Value = string.Empty;
                                else
                                {
                                    string[] lsArr = lsMoreInfo.Split(';');
                                    if (lsArr.Length > 0) hdnCountry.Value = lsArr[0];
                                    if (lsArr.Length > 1) hdnEBMName.Value = lsArr[1];
                                    if (lsArr.Length > 2) hdnProspectName.Value = lsArr[2];
                                }
                                //SetMoreInfo(itemNo, lsMoreInfo);
                                //this.lvStage.DataSource = dv;
                                //this.lvStage.EditIndex = ds.Tables[0].Rows.Count - 1;
                                //this.lvStage.DataBind();
                            } break;

                        default:
                            break;
                    }
                    Session["chosenMoreInfo"] = null;
                }
                // set day name of week in header
                UpdateListViewHeader();
                EnableButton(); // disable or enable Edit, Delete buttons depend on status
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
            }
            txtFromDate.Text = FromDate;
        }
        #endregion

        protected void lnkMoreInfo_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewDataItem lvi = (ListViewDataItem)((LinkButton)sender).Parent;
                string lsMoreInfo = string.Empty;
                if (hdnStatus.Value.Equals("Editing") || hdnStatus.Value.Equals("Adding"))
                    lsMoreInfo = hdnCountry.Value + ";" + hdnEBMName.Value + ";" + hdnProspectName.Value;
                else //if (hdnStatus.Value.Equals("Adding"))
                {
                    if(((Label)lvi.FindControl("lblMoreInfo")) != null)
                        lsMoreInfo = ((Label)lvi.FindControl("lblMoreInfo")).Text;
                }

                string popup = "OpenMoreInfo('" + lsMoreInfo + "','" + hdnStatus.Value +"')";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "CallJS", popup, true);
            }
            catch (Exception ex)
            {
                
            }
        }

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (PeriodStatus == 1) return;
            UpdateStatus();
        }
        #endregion

        protected void ddlPrj_SelectedIndexChanged(object sender, EventArgs e)
        {

        } 
        #endregion

        #region Methods

        #region Binding
        void Binding()
        {
            txtUserCode.Text = User.Identity.Name;
            txtUserCode.ReadOnly = true;

            CultureInfo ivC = new CultureInfo("es-US");
            DateTime Today = Convert.ToDateTime(FromDate, ivC);
            Monday = Today.AddDays(1 - Today.DayOfWeek.GetHashCode());
            Tuesday = Today.AddDays(2 - Today.DayOfWeek.GetHashCode());
            Wednesday = Today.AddDays(3 - Today.DayOfWeek.GetHashCode());
            Thursday = Today.AddDays(4 - Today.DayOfWeek.GetHashCode());
            Friday = Today.AddDays(5 - Today.DayOfWeek.GetHashCode());
            Saturday = Today.AddDays(6 - Today.DayOfWeek.GetHashCode());
            Sunday = Today.AddDays(7 - Today.DayOfWeek.GetHashCode());

            DataSet ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_WeekendTimeSheet",
                Data.CreateParameter("@IN_BeginDate", Monday.ToString("MM/dd/yyyy")), Data.CreateParameter("@IN_UserCode", User.Identity.Name));
            // Keep ds
            Session["dsWeeklyTimesheet"] = ds;

            string lsSQL = " Select Count(*) From tbl_TimeEntry Where UserCode = '" + User.Identity.Name + "' and [Status] = 0 "
                + " and convert(varchar(8), [Date], 112) >=  '" + Monday.ToString("yyyyMMdd") + "'"
                + " and convert(varchar(8), [Date], 112) <=  '" + Sunday.ToString("yyyyMMdd") + "'";

            if (!CheckStatus(lsSQL) && ds.Tables[0].Rows.Count > 0)
            {
                //btnAddRecord.Enabled = false;
                btnSubmit.Enabled = false;
            }

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
            LoadPrj();

            if (ds.Tables[0].Rows.Count <= 0)
                btnSubmit.Enabled = false;
        }
        #endregion

        #region LoadPrj
        private void LoadPrj()
        {
            dsPrjList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
        }
        #endregion

        #region AddUpdateItem
        private void AddUpdateItem(Int32 itemNo, Project chosenItem)
        {
            if (chosenItem != null)
            {
                DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
                Int32 CurNo = 0;

                List<string> lstType = new List<string>();

                lstType.Add("ADMIN");
                lstType.Add("OTHER");
                lstType.Add("PRESALES");

                DataRow drX;
                if (itemNo == 0)
                {
                    drX = ds.Tables[0].NewRow();
                    CurNo = ds.Tables[0].Rows.Count + 1;
                    drX["No"] = CurNo;
                }
                else
                    drX = ds.Tables[0].Rows[itemNo - 1];

                string lsPrjCode = string.Empty, lsPrjName = string.Empty, lsSAPB1DB = string.Empty, lsTask = string.Empty, lsComments = string.Empty;
                string lsSrvType = string.Empty, lsTimeEndDate = string.Empty;
                int iNo = ds.Tables[0].Rows.Count + 1;

                if (dsPrjList != null && dsPrjList.Tables != null && dsPrjList.Tables[0].Rows.Count > 0)
                {
                    string[] prjArr = chosenItem.PrjCode.ToString().Split(';');
                    lsPrjName = chosenItem.PrjName;
                    lsSrvType = chosenItem.SrvType;
                    lsTimeEndDate = chosenItem.TimeEndDate;
                    lsPrjCode = prjArr[0];
                    lsSAPB1DB = prjArr[1];
                }

                drX["No"] = iNo.ToString();
                drX["IDS"] = "0;0;0;0;0;0;0";

                drX["UserCode"] = User.Identity.Name;
                drX["Client"] = "";
                drX["PrjCode"] = lsPrjCode;
                drX["OnSite"] = "Yes";
                drX["PrjName"] = lsPrjName;

                drX["Status"] = 0;
                drX["SAPB1DB"] = lsSAPB1DB;
                drX["Task"] = "";
                drX["Comments"] = lsComments;
                drX["Mon"] = 0;
                drX["Tue"] = 0;
                drX["Wed"] = 0;
                drX["Thu"] = 0;
                drX["Fri"] = 0;
                drX["Sat"] = 0;
                drX["Sun"] = 0;
                drX["Total"] = 0;
                drX["Billable"] = 0;
                drX["ServiceType"] = lsSrvType;
                drX["U_AI_TimeEndDate"] = lsTimeEndDate;
                drX["Phase"] = msPhase = lstType.Contains(lsSrvType) ? "Others" : "Project Preparation";
                drX["MoreInfo"] = ";;";

                if (itemNo == 0)
                    ds.Tables[0].Rows.Add(drX);
            }
        }
        #endregion

        private void SetMoreInfo(Int32 itemNo, string moreInfo)
        {
            DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
            DataRow[] rows = ds.Tables[0].Select("No=" + itemNo.ToString());
            if (rows != null && rows.Length > 0)
            {
                rows[0]["MoreInfo"] = moreInfo;
            }
        }

        private bool DataValidation(int currRow, ListViewItem lvi)
        {
            bool smooth = true;
            string lsSrvType = ((TextBox)lvi.FindControl("txtSrvTypeEdit")).Text;
            string lsSAPB1DB = ((Label)lvi.FindControl("lblSAPB1DBEdit")).Text;
            string lsPrjCode = ((Label)lvi.FindControl("lblPrjCodeEdit")).Text;
            int li = 0;
            if (lsSrvType.CompareTo("HLPDSK") > 0) // Check service type by 
            {
                decimal decMon = 0, decTue = 0, decWed = 0, decThu = 0, decFri = 0, decSat = 0, decSun = 0;
                // Get Hours
                decMon = CS2Dec(((TextBox)lvi.FindControl("txtMonEdit")).Text);
                decTue = CS2Dec(((TextBox)lvi.FindControl("txtTueEdit")).Text);
                decWed = CS2Dec(((TextBox)lvi.FindControl("txtWedEdit")).Text);
                decThu = CS2Dec(((TextBox)lvi.FindControl("txtthuEdit")).Text);
                decFri = CS2Dec(((TextBox)lvi.FindControl("txtFriEdit")).Text);
                decSat = CS2Dec(((TextBox)lvi.FindControl("txtSatEdit")).Text);
                decSun = CS2Dec(((TextBox)lvi.FindControl("txtSunEdit")).Text);

                DataSet dsDBList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
                DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (currRow == li)
                    {
                        li++;
                        continue;
                    }

                    if (dsDBList != null && dsDBList.Tables.Count > 0)
                    {
                        DataRow[] rows = dsDBList.Tables[0].Select("ProjectCodeOnly = '" + row["PrjCode"].ToString() + "'");
                        if (rows.Length > 0)
                            if (rows[0]["U_AI_SrvType"].ToString().Equals("HLPDSK"))
                            {
                                li++;
                                continue;
                            }
                    }

                    // Get Hours
                    // if (lsPrjCode.CompareTo(row["PrjCode"].ToString()) == 0 && lsSAPB1DB.CompareTo(row["SAPB1DB"].ToString()) == 0)
                    {
                        string lsMon = row["Mon"].ToString();
                        string lsTue = row["Tue"].ToString();
                        string lsWed = row["Wed"].ToString();
                        string lsThu = row["Thu"].ToString();
                        string lsFri = row["Fri"].ToString();
                        string lsSat = row["Sat"].ToString();
                        string lsSun = row["Sun"].ToString();
                        decMon = decMon + CS2Dec(lsMon);
                        decTue = decTue + CS2Dec(lsTue);
                        decWed = decWed + CS2Dec(lsWed);
                        decThu = decThu + CS2Dec(lsThu);
                        decFri = decFri + CS2Dec(lsFri);
                        decSat = decSat + CS2Dec(lsSat);
                        decSun = decSun + CS2Dec(lsSun);
                    }
                    li++;
                }
                if (decMon > 8 || decTue > 8 || decWed > 8 || decThu > 8 || decFri > 8 || decSat > 8 || decSun > 8)
                {
                    lblMessage.Text = "This is not Help Desk service type. Cannot key-in more than 8 hours per day.";
                    smooth = false;
                }
            }

            // Check project code relation with SAPB1DB
            if (smooth)
            {
                smooth = CheckBelongCompany(lsPrjCode, lsSAPB1DB);
                /*DataSet dsDBList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBListByConsultant",
                    Data.CreateParameter("@IN_UserCode", User.Identity.Name));
                List<string> DBList = new List<string>();
                foreach (DataRow row in dsDBList.Tables[0].Rows)
                {
                    DBList.Add(row["DBName"].ToString());
                }

                if (DBList == null || DBList.Count <= 0 || !DBList.Contains(lsSAPB1DB))
                {
                    lblMessage.Text = "The '" + lsPrjCode +"' project is invalid. You don't belongs '" + lsSAPB1DB + "' company.";
                    smooth = false;
                }*/
            }

            return smooth;
        }

        private bool CheckBelongCompany(string asPrjCode, string asSAPB1DB)
        {
            bool smooth = true;
            DataSet dsDBList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetDBListByConsultant",
                Data.CreateParameter("@IN_UserCode", User.Identity.Name));
            List<string> DBList = new List<string>();
            foreach (DataRow row in dsDBList.Tables[0].Rows)
            {
                DBList.Add(row["DBName"].ToString());
            }

            if (DBList == null || DBList.Count <= 0 || !DBList.Contains(asSAPB1DB))
            {

                lblMessage.Text = "The '" + asPrjCode + "' project is invalid. You don't belongs '" + asSAPB1DB + "' company.";
                lblMessage.Visible = true;
                SalesOpportunityUpdatePanel.Update();
                smooth = false;
            }
            else
            {
                lblMessage.Text = "";
            }

            return smooth;
        }

        #region Save
        private void Save(string[] asIDSArr, ListViewItem lvi)
        {
            if (asIDSArr.Length < 7) return;
            
            string lsSAPB1DB = ((Label)lvi.FindControl("lblSAPB1DBEdit")).Text;// lsPrjSAPB1DBArr[1];
            string lsPrjCode = ((Label)lvi.FindControl("lblPrjCodeEdit")).Text;// lsPrjSAPB1DBArr[0];
            string lsPrjName = ((Label)lvi.FindControl("lblPrjNameEdit")).Text;//((DropDownList)lvi.FindControl("ddlPrjTaskEdit")).SelectedItem.Text;
            string lsTask = ((TextBox)lvi.FindControl("txtTaskEdit")).Text;
            string lsComments = ((TextBox)lvi.FindControl("txtCommentsEdit")).Text;
            string lsPhase = ((DropDownList)lvi.FindControl("ddlPhaseEdit")).SelectedValue;
            //string[] lsMoreInfo = ((Label)lvi.FindControl("lblMoreInfoEdit")).Text.Split(';');
            //string lsCountry = string.Empty, lsEBMName = string.Empty, lsProspectName = string.Empty;
            //if (lsMoreInfo != null)
            //{
            //    if (lsMoreInfo.Length > 0) lsCountry = lsMoreInfo[0];
            //    if (lsMoreInfo.Length > 1) lsEBMName = lsMoreInfo[1];
            //    if (lsMoreInfo.Length > 2) lsProspectName = lsMoreInfo[2];
            //}
            // Get Hours
            string lsMon = ((TextBox)lvi.FindControl("txtMonEdit")).Text;
            string lsTue = ((TextBox)lvi.FindControl("txtTueEdit")).Text;
            string lsWed = ((TextBox)lvi.FindControl("txtWedEdit")).Text;
            string lsThu = ((TextBox)lvi.FindControl("txtthuEdit")).Text;
            string lsFri = ((TextBox)lvi.FindControl("txtFriEdit")).Text;
            string lsSat = ((TextBox)lvi.FindControl("txtSatEdit")).Text;
            string lsSun = ((TextBox)lvi.FindControl("txtSunEdit")).Text;

            // Get Billable
            int liBillable = (((CheckBox)lvi.FindControl("chkBillableEdit")).Checked == true ? 1 : 0);

            SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_InsUpdTimeEntry",
                Data.CreateParameter("@IN_UserCode", User.Identity.Name),
                Data.CreateParameter("@IN_Monday", Monday.ToString("yyyyMMdd")),
                Data.CreateParameter("@IN_PrjCode", lsPrjCode),
                Data.CreateParameter("@IN_PrjName", lsPrjName),
                Data.CreateParameter("@IN_Task", lsTask),
                Data.CreateParameter("@IN_Comments", lsComments),
                Data.CreateParameter("@IN_SAPB1DB", lsSAPB1DB),
                Data.CreateParameter("@IN_MON", asIDSArr[0] + ";" + CS2Dec(lsMon).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_TUE", asIDSArr[1] + ";" + CS2Dec(lsTue).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_WED", asIDSArr[2] + ";" + CS2Dec(lsWed).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_THU", asIDSArr[3] + ";" + CS2Dec(lsThu).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_FRI", asIDSArr[4] + ";" + CS2Dec(lsFri).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_SAT", asIDSArr[5] + ";" + CS2Dec(lsSat).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_SUN", asIDSArr[6] + ";" + CS2Dec(lsSun).ToString() + ";" + liBillable.ToString()),
                Data.CreateParameter("@IN_PHASE", lsPhase),
                Data.CreateParameter("@IN_COUNTRY", hdnCountry.Value),
                Data.CreateParameter("@IN_EBMNAME", hdnEBMName.Value),
                Data.CreateParameter("@IN_PROSPECTNAME", hdnProspectName.Value)
                );
            //FromDate = txtFromDate.Text.Trim();
            Response.Redirect(WeeklyURL + "&date=" + txtFromDate.Text.Trim());
        }
        #endregion

        #region UpdateStatus
        private void UpdateStatus()
        {
            DataSet ds = (DataSet)Session["dsWeeklyTimesheet"];
            if (ds != null & ds.Tables != null & ds.Tables[0].Rows.Count > 0)
            {
                string lsIDS = string.Empty;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dr["Status"].ToString().Equals("0")) continue;

                    if (lsIDS.Equals(""))
                        lsIDS = dr["IDS"].ToString();
                    else
                        lsIDS = lsIDS + ";" + dr["IDS"].ToString();
                }
                if (!lsIDS.Equals(""))
                {
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_UpdateTimeEntryStatus", Data.CreateParameter("@IN_IDS", lsIDS), Data.CreateParameter("@IN_Status", "2"));
                    Response.Redirect(WeeklyURL + "&date=" + txtFromDate.Text.Trim());
                }
            }
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
            CultureInfo ivCEn = new CultureInfo("en-US");
            Label lblMon = this.lvStage.Controls[0].FindControl("lblMon") as Label;
            if (lblMon != null) lblMon.Text = Monday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblTue = this.lvStage.Controls[0].FindControl("lblTue") as Label;
            if (lblTue != null) lblTue.Text = Tuesday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblWed = this.lvStage.Controls[0].FindControl("lblWed") as Label;
            if (lblWed != null) lblWed.Text = Wednesday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblThu = this.lvStage.Controls[0].FindControl("lblThu") as Label;
            if (lblThu != null) lblThu.Text = Thursday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblFri = this.lvStage.Controls[0].FindControl("lblFri") as Label;
            if (lblFri != null) lblFri.Text = Friday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblSat = this.lvStage.Controls[0].FindControl("lblSat") as Label;
            if (lblSat != null) lblSat.Text = Saturday.ToString("ddd </br> dd-MMM-yy", ivCEn);

            Label lblSun = this.lvStage.Controls[0].FindControl("lblSun") as Label;
            if (lblSun != null) lblSun.Text = Sunday.ToString("ddd </br> dd-MMM-yy", ivCEn);
        }
        #endregion

        #region CheckStatus
        private bool CheckStatus(string asSQL)
        {
            SqlDataReader reader = (SqlDataReader)SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.Text, asSQL);
            reader.Read();
            if (int.Parse(reader[0].ToString()) > 0) return true;

            return false;
        }
        #endregion

        #region EnableButton
        private void EnableButton()
        {
            for (int i = 0; i < lvStage.Items.Count; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                Label lblStatus = (Label)lvi.FindControl("lblStatus");
                if (lblStatus == null) continue;

                if (!lblStatus.Text.Equals("0"))
                {
                    int idx = lvi.Controls.IndexOf((LinkButton)lvi.FindControl("imgbEdit"));
                    LinkButton lbtnEdit = (LinkButton)lvi.FindControl("imgbEdit");
                    LinkButton lbtnDelete = (LinkButton)lvi.FindControl("imgbDelete");
                    lvi.Controls.Remove(lbtnEdit);
                    lvi.Controls.Remove(lbtnDelete);

                    Label lblStatusName = new Label();
                    lblStatusName.ID = "lblStatusName" + i.ToString();
                    if (lblStatus.Text.Equals("1"))
                        lblStatusName.Text = "Approved";
                    else if (lblStatus.Text.Equals("2"))
                        lblStatusName.Text = "Pending";

                    //lblStatusName.Font.Bold = true;
                    lvi.Controls.AddAt(idx, lblStatusName);
                }
            }
        }
        #endregion

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FromDate = txtFromDate.Text.Trim();
            //mbVisible = true; // Default value
            Response.Redirect(WeeklyURL + "&date=" + txtFromDate.Text.Trim());
        }

        public void GeneratePeriodStatus()
        {
            string lsSql = "Select Date_Locked From dbo.tbl_TimsheetPostingPeriod Where convert(varchar(6), Starting_Date, 112) = convert(varchar(6), cast('" + FromDate + "' As date) , 112) ";
            // Check period Locked or Unlocked.
            SqlDataReader reader = (SqlDataReader)SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.Text, lsSql);
            reader.Read();
            
            if (reader.HasRows && bool.Parse(reader[0].ToString()) ) PeriodStatus = 1;

            StringBuilder JSBuilder = new StringBuilder();
            JSBuilder.AppendLine("<script language='javascript'>");
            JSBuilder.AppendLine("var PeriodStatus = " + PeriodStatus.ToString() + ";");
            JSBuilder.AppendLine("</script>");

            // done, generate on the page
            if (!this.ClientScript.IsClientScriptBlockRegistered("GeneratePeriodStatus"))
            {
                this.ClientScript.RegisterClientScriptBlock(Type.GetType("System.String"), "GeneratePeriodStatus", JSBuilder.ToString());
            }
        }

        #endregion

        protected void lvStage_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            if (PeriodStatus == 1)
            {
                e.Cancel = true;
                return;
            }
        }

        protected void lvStage_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem lvi = lvStage.Items[e.ItemIndex];
            if (!DataValidation(e.ItemIndex, lvi))
            {
                lblMessage.Visible = true;
                //lblMessage.Text = "This is not Help Desk service. Cannot key-in more than 8 hours per day.";
                e.Cancel = true;
            }
        }
    }
}