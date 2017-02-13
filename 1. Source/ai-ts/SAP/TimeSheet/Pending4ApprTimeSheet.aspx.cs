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
    public partial class Pending4ApprTimeSheet : System.Web.UI.Page
    {
        private static int mi_idxUser = 0;
        private static int mi_idxStatus = 0;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ds = new DataSet();
                if (Request.QueryString["date"] != null && Request.QueryString["date"].Length == 10)
                {
                    txtFromDate.Text = Request.QueryString["date"];
                }
                else
                {
                    txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                Binding();
            }
        }

        void Binding()
        {            
            CultureInfo ivC = new CultureInfo("es-US");
            DateTime Today = Convert.ToDateTime(txtFromDate.Text, ivC);
            Monday = Today.AddDays(1 - Today.DayOfWeek.GetHashCode());
            Tuesday = Today.AddDays(2 - Today.DayOfWeek.GetHashCode());
            Wednesday = Today.AddDays(3 - Today.DayOfWeek.GetHashCode());
            Thursday = Today.AddDays(4 - Today.DayOfWeek.GetHashCode());
            Friday = Today.AddDays(5 - Today.DayOfWeek.GetHashCode());
            Saturday = Today.AddDays(6 - Today.DayOfWeek.GetHashCode());
            Sunday = Today.AddDays(7 - Today.DayOfWeek.GetHashCode());
            ////////////////////////////////////////////////////////////////////
            LoadUser();
            ddlUser.DataSource = dsUserList.Tables[0];
            ddlUser.DataTextField = "UserCode";
            ddlUser.DataValueField = "UserCode";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "");
            ddlUser.SelectedIndex = mi_idxUser;
            ddlStatus.SelectedIndex = mi_idxStatus;
            /////////////////////////////////////////////////////////////////////

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_Pending4ApproveTimeSheet",
                Data.CreateParameter("@IN_BeginDate", Monday.ToString("MM/dd/yyyy")),
                Data.CreateParameter("@IN_ProjectManager", User.Identity.Name),
                Data.CreateParameter("@IN_UserCode", ddlUser.SelectedValue),
                Data.CreateParameter("@IN_Status", ddlStatus.SelectedValue));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
            LoadPrj();

            if (ds.Tables[0].Rows.Count <= 0)
            {
                btnReject.Enabled = 
                btnApprove.Enabled = false;
            }
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            // Label lblIDS;
            switch (e.CommandName)
            {
                case "Update":
                    // lblIDS = (Label)lvi.FindControl("lblIDSEdit");
                    // Save(lblIDS.Text.Split(';'), lvi);
                    break;

                case "Edit":
                    btnApprove.Enabled = false;
                    txtFromDate.Enabled = false;
                    break;

                case "Delete":
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
                        Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
                    } break;

                case "Cancel":
                    Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            Binding();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            UpdateListViewHeader();
            EnableChkSelection();
        }

        protected void lvStage_ItemCreated(object sender, ListViewItemEventArgs e)
        {

        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)lvStage.FindControl("chkAll");
            SetCheckBox(chkAll.Checked);
        }

        private void SetCheckBox(bool abFlag)
        {
            foreach (ListViewItem lvi in lvStage.Items)
            {
                ((CheckBox)lvi.FindControl("Selection")).Checked = abFlag;
            }
        }

        private void LoadUser()
        {
            dsUserList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, "Select distinct UserCode "
                + "From tbl_TimeEntry "
                + "where Convert(varchar(8), [Date], 112) >= '" + Monday.ToString("yyyyMMdd") + "'"
                + "  and Convert(varchar(8), [Date], 112) <= '" + Sunday.ToString("yyyyMMdd") + "' and [Status] in (2, 1) "
                + "  and PrjCode in (Select SUBSTRING(PrjCode, 1, CHARINDEX(';',PrjCode)-1) "
                + " 				 From dbo.tbl_ProjectManager P inner join dbo.aspnet_Users U on P.UserID = U.UserID "
                + " 				where U.UserName = '" + User.Identity.Name + "') ");
                
        }

        private void LoadPrj()
        {
            dsPrjList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
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

        protected void lvStage_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void lvStage_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lvStage_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("1");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateStatus("0");
        }

        private void Save(string[] asIDSArr, ListViewItem lvi)
        {
            if (asIDSArr.Length < 7) return;

            string[] lsPrjSAPB1DBArr = ((DropDownList)lvi.FindControl("ddlPrjTaskEdit")).SelectedItem.Value.Split(';');
            string lsSAPB1DB = lsPrjSAPB1DBArr[1];
            string lsPrjCode = lsPrjSAPB1DBArr[0];
            string lsPrjName = ((DropDownList)lvi.FindControl("ddlPrjTaskEdit")).SelectedItem.Text;
            string lsTask = ((TextBox)lvi.FindControl("txtTaskEdit")).Text;
            string lsComments = ((TextBox)lvi.FindControl("txtCommentsEdit")).Text;

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
                Data.CreateParameter("@IN_PHASE", "")               
                );
            Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim()); 
        }

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
                    
                    //if (((Label)lvi.FindControl("lblStatus")).Text.ToString().Equals("2"))
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
                    Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
                }
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            mi_idxUser = ddlUser.SelectedIndex;
            mi_idxStatus = ddlStatus.SelectedIndex;
            Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
        }

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            mi_idxUser = ddlUser.SelectedIndex;
            mi_idxStatus = ddlStatus.SelectedIndex;
            Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            if (lvStage.EditIndex < 0) return;
            ListViewItem lvi = lvStage.Items[lvStage.EditIndex];

            string lsMon = ((TextBox)lvi.FindControl("txtMonEdit")).Text;
            string lsTue = ((TextBox)lvi.FindControl("txtTueEdit")).Text;
            string lsWed = ((TextBox)lvi.FindControl("txtWedEdit")).Text;
            string lsThu = ((TextBox)lvi.FindControl("txtThuEdit")).Text;
            string lsFri = ((TextBox)lvi.FindControl("txtFriEdit")).Text;
            string lsSat = ((TextBox)lvi.FindControl("txtSatEdit")).Text;
            string lsSun = ((TextBox)lvi.FindControl("txtSunEdit")).Text;
            decimal total = CS2Dec(lsMon) + CS2Dec(lsTue) + CS2Dec(lsWed) + CS2Dec(lsThu) + CS2Dec(lsFri) + CS2Dec(lsSat) + CS2Dec(lsSun);
            ((Label)lvi.FindControl("lblTotalEdit")).Text = total.ToString();

            SetFocus(sender, lvi);

        }

        protected void ddlPrj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStage.EditIndex < 0) return;
            string prj = ((DropDownList)sender).SelectedValue;
            string[] prjArr = prj.Split(';');

            if (prjArr.Length > 0)
            {
                ListViewItem lvi = lvStage.Items[lvStage.EditIndex];
                ((Label)lvi.FindControl("lblPrjCodeEdit")).Text = prjArr[0];
                ds.Tables[0].Rows[lvStage.EditIndex]["PrjCode"] = prjArr[0];
                ds.Tables[0].Rows[lvStage.EditIndex]["SAPB1DB"] = prjArr[1];
            }  
        }

        private void SetFocus(object sender, ListViewItem avi)
        {
            switch (((TextBox)sender).ID)
            {
                case "txtMonEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtTueEdit")).Focus();
                    break;
                case "txtTueEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtWedEdit")).Focus();
                    break;
                case "txtWedEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtThuEdit")).Focus();
                    break;
                case "txtThuEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtFriEdit")).Focus();
                    break;
                case "txtFriEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtSatEdit")).Focus();
                    break;
                case "txtSatEdit":
                    lvStage.Focus();
                    ((TextBox)avi.FindControl("txtSunEdit")).Focus();
                    break;
            }
        }

        private void Resettext(ListViewItem avi)
        {
            ((TextBox)avi.FindControl("txtTaskEdit")).Text =
            ((TextBox)avi.FindControl("txtMonEdit")).Text =
            ((TextBox)avi.FindControl("txtTueEdit")).Text =
            ((TextBox)avi.FindControl("txtWedEdit")).Text =
            ((TextBox)avi.FindControl("txtThuEdit")).Text =
            ((TextBox)avi.FindControl("txtFriEdit")).Text =
            ((TextBox)avi.FindControl("txtSatEdit")).Text =
            ((TextBox)avi.FindControl("txtSunEdit")).Text = 
            ((TextBox)avi.FindControl("txtCommentsEdit")).Text = string.Empty;
            ((TextBox)avi.FindControl("txtTaskEdit")).Focus();
        }

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

        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {
            decimal ldecMonTotal = 0, ldecTueTotal = 0, ldecWedTotal = 0, ldecThuTotal = 0, ldecFriTotal = 0, ldecSatTotal = 0, ldecSunTotal = 0, ldecAllTotal = 0;

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

        private int CheckStatus(string asSQL)
        {
            SqlDataReader reader = (SqlDataReader)SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.Text, asSQL);
            reader.Read();
            return int.Parse(reader[0].ToString());
        }

        //private void EnableDetails(bool abVisible)
        //{
        //    System.Web.UI.HtmlControls.HtmlControl th = (System.Web.UI.HtmlControls.HtmlControl)lvStage.FindControl("thButtons");
        //    if (th != null) th.Visible = abVisible;

        //    System.Web.UI.HtmlControls.HtmlControl tf = (System.Web.UI.HtmlControls.HtmlControl)lvStage.FindControl("tfEmpty");
        //    if (tf != null) tf.Visible = abVisible;
        //}

        private void EnableChkSelection()
        {
            for (int i = 0; i < lvStage.Items.Count; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                if (((Label)lvi.FindControl("lblStatus")).Text.ToString().Equals("1"))
                {
                    ((CheckBox)lvi.FindControl("Selection")).Text = "Approved";
                    //int idx = lvi.Controls.IndexOf((CheckBox)lvi.FindControl("Selection"));
                    //lvi.Controls.Remove((CheckBox)lvi.FindControl("Selection"));
                    //Label lblAppr = new Label();
                    //lblAppr.ID = "lblAppr" + i.ToString();
                    //lblAppr.Text = "Approved";
                    //lblAppr.Font.Bold = true;
                    //lvi.Controls.AddAt(idx, lblAppr);
                }
                else if (((Label)lvi.FindControl("lblStatus")).Text.ToString().Equals("2"))
                {
                    ((CheckBox)lvi.FindControl("Selection")).Text = "Pending";
                }
            }
        }

        private void DisplayChkStatus()
        {
            if (ds.Tables[0].Rows.Count <= 0) return;
            CheckBox chkAll = (CheckBox)this.lvStage.Controls[0].FindControl("chkAll");
            int idx = this.lvStage.Controls[0].Controls.IndexOf(chkAll);
            this.lvStage.Controls[0].Controls.Remove(chkAll);
            Label lblStatus = new Label();
            lblStatus.Text = "Status";
            this.lvStage.Controls[0].Controls.AddAt(idx, lblStatus);
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            mi_idxUser = ddlUser.SelectedIndex;
            mi_idxStatus = ddlStatus.SelectedIndex;
            Response.Redirect("/TimeSheet/Pending4ApprTimeSheet.aspx?date=" + txtFromDate.Text.Trim());
        }
    }
}