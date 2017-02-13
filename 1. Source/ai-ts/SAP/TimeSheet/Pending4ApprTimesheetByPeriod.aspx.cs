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
    public partial class Pending4ApprTimeSheetByPeriod : System.Web.UI.Page
    {
        #region Properties
        #region dsPrjList
        protected DataSet dsPrjList
        {
            get { return (DataSet)ViewState["dsPrjList"]; }
            set { ViewState["dsPrjList"] = value; }
        }
        #endregion

        #region dsUserList
        protected DataSet dsUserList
        {
            get { return (DataSet)ViewState["dsUserList"]; }
            set { ViewState["dsUserList"] = value; }
        }
        #endregion

        #region ds
        protected DataSet ds
        {
            get { return (DataSet)ViewState["ds"]; }
            set { ViewState["ds"] = value; }
        }
        #endregion
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ds = new DataSet();
                if (Request.QueryString["fromdate"] != null && Request.QueryString["fromdate"].Length == 10 
                    && Request.QueryString["todate"] != null && Request.QueryString["todate"].Length == 10 ) // set lastest from date and to date
                {
                    txtFromDate.Text = Request.QueryString["fromdate"];
                    txtToDate.Text = Request.QueryString["todate"];

                    BindingUser();
                    ddlUser.SelectedIndex = int.Parse(Request.QueryString["UserIdx"]);
                    ddlStatus.SelectedIndex = int.Parse(Request.QueryString["StatusIdx"]);
                }
                else // set default first loading
                {
                    txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                    txtToDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");
                    BindingUser();
                }
                Binding();
            }
        }

        protected void BindingUser()
        {
            LoadUser();
            ddlUser.DataSource = dsUserList.Tables[0];
            ddlUser.DataTextField = "UserCode";
            ddlUser.DataValueField = "UserCode";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "");
        }

        protected void Binding()
        {            
            CultureInfo ivC = new CultureInfo("es-US");
            DateTime Fromdate = Convert.ToDateTime(txtFromDate.Text, ivC);
            DateTime Todate = Convert.ToDateTime(txtToDate.Text, ivC);

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_Pending4ApproveTimeSheetByPeriod",
                Data.CreateParameter("@IN_BeginDate", Fromdate.ToString("MM/dd/yyyy")), Data.CreateParameter("@IN_EndDate", Todate.ToString("MM/dd/yyyy")),
                Data.CreateParameter("@IN_ProjectManager", User.Identity.Name), Data.CreateParameter("@IN_UserCode", ddlUser.SelectedValue),
                Data.CreateParameter("@IN_Status", ddlStatus.SelectedValue));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();

            if (ds.Tables[0].Rows.Count <= 0)
            {
                btnReject.Enabled =
                btnApprove.Enabled = false;
            }
            else
            {
                btnReject.Enabled =
                btnApprove.Enabled = true;            
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
            base.OnLoadComplete(e);
            CalculateTotal();
            EnableChkSelection();
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)lvStage.FindControl("chkAll");
            SetCheckBox(chkAll.Checked);
        }

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
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //Binding();
            Response.Redirect("/TimeSheet/Pending4ApprTimesheetByPeriod.aspx?fromdate=" + txtFromDate.Text.Trim() + "&todate=" + txtToDate.Text.Trim() + "&UserIdx=" + ddlUser.SelectedIndex.ToString() + "&StatusIdx=" + ddlStatus.SelectedIndex.ToString());
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("1");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateStatus("0");
        }
        #endregion

        #region Methods

        #region SetCheckBox
        private void SetCheckBox(bool abFlag)
        {
            foreach (ListViewItem lvi in lvStage.Items)
            {
                ((CheckBox)lvi.FindControl("Selection")).Checked = abFlag;
            }
        }
        #endregion

        #region LoadUser
        private void LoadUser()
        {
            CultureInfo ivC = new CultureInfo("es-US");
            DateTime Fromdate = Convert.ToDateTime(txtFromDate.Text, ivC);
            DateTime Todate = Convert.ToDateTime(txtToDate.Text, ivC);

            dsUserList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, "Select distinct UserCode "
                + " From tbl_TimeEntry "
                + "where Convert(varchar(8), [Date], 112) >= '" + Fromdate.ToString("yyyyMMdd") + "'"
                + "  and Convert(varchar(8), [Date], 112) <= '" + Todate.ToString("yyyyMMdd")
                + "' and [Status] in (2, 1) "
                + "  and PrjCode in (Select SUBSTRING(PrjCode, 1, CHARINDEX(';',PrjCode)-1) "
                + " 				 From dbo.tbl_ProjectManager P inner join dbo.aspnet_Users U on P.UserID = U.UserID "
                + " 				Where U.UserName = '" + User.Identity.Name + "') ");
        }
        #endregion

        #region UpdateStatus
        private void UpdateStatus(string as_Status)
        {
            if (lvStage != null & lvStage.Items != null & lvStage.Items.Count > 0)
            {
                string lsIDS = string.Empty;

                for (int i = 0; i < lvStage.Items.Count; i++)
                {
                    ListViewItem lvi = lvStage.Items[i];
                    CheckBox chkSelection = (CheckBox)lvi.FindControl("Selection");

                    if (chkSelection == null || !chkSelection.Checked) continue;
                    
                    {
                        Label lblIDS = ((Label)lvi.FindControl("lblIDS"));
                        if (lblIDS == null) continue;
                        if (lsIDS.Equals(""))
                            lsIDS = lblIDS.Text;
                        else
                            lsIDS = lsIDS + ";" + lblIDS.Text;
                    }
                }

                if (!lsIDS.Equals(""))
                {
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_UpdateTimeEntryStatus", Data.CreateParameter("@IN_IDS", lsIDS), Data.CreateParameter("@IN_Status", as_Status));
                    Response.Redirect("/TimeSheet/Pending4ApprTimesheetByPeriod.aspx?fromdate=" + txtFromDate.Text.Trim() + "&todate=" + txtToDate.Text.Trim() + "&UserIdx=" + ddlUser.SelectedIndex.ToString() + "&StatusIdx=" + ddlStatus.SelectedIndex.ToString());
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

        #region EnableChkSelection
        private void EnableChkSelection()
        {
            for (int i = 0; i < lvStage.Items.Count; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                if (((Label)lvi.FindControl("lblStatus")).Text.ToString().Equals("1"))
                {
                    ((CheckBox)lvi.FindControl("Selection")).Text = "Approved";
                }
                else if (((Label)lvi.FindControl("lblStatus")).Text.ToString().Equals("2"))
                {
                    ((CheckBox)lvi.FindControl("Selection")).Text = "Pending";
                }
            }
        }
        #endregion

        #region CalculateTotal
        protected void CalculateTotal()
        {
            decimal ldecHourTotal = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ldecHourTotal += CS2Dec(dr["Hour"].ToString());
            }
            Label lblHourTotal = (Label)lvStage.FindControl("lblHourTotal");
            if (lblHourTotal != null) lblHourTotal.Text = ldecHourTotal.ToString();
        }
        #endregion

        #endregion

    }
}