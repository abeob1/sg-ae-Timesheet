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
    public partial class ProjectManager : System.Web.UI.Page
    {
        private static DataSet ds = new DataSet();
        private static DataSet dsPrjList = new DataSet();
        private static DataSet dsUserList = new DataSet();
        //private static int mi_idx = 0;
        private static int mi_counter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Binding();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Panel), "contentData", ";ScrollToBottom();", true); // Always scroll to bottom page
        }

        void Binding()
        {
            LoadPrj("");
            LoadUser();

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectManager", Data.CreateParameter("@IN_UserCode", ""));

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            Label lblID;
            switch (e.CommandName)
            {
                case "Update":
                    lblID = (Label)lvi.FindControl("lblIDEdit");
                    Save(lblID.Text, lvi);
                    break;

                case "Edit":
                    btnAddRecord.Enabled = false;
                    break;

                case "Delete":
                    int li_ErrorCode = 0;
                    try
                    {
                        string lsID = dr["ID"].ToString();

                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Delete tbl_ProjectManager Where ID = " + lsID + "");
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
                    LoadPrj("");
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
            base.OnLoadComplete(e);
        }

        protected void lvStage_ItemCreated(object sender, ListViewItemEventArgs e)
        {

        }

        private void LoadPrj(string aPrjCode)
        {
            dsPrjList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_GetProjectList", Data.CreateParameter("@IN_PrjCode", aPrjCode));
        }

        private void LoadUser()
        {
            dsUserList = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, "Select UserId, UserName From aspnet_Users");
        }

        protected void lvStage_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvStage.EditIndex >= 0)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                if (dataItem.DisplayIndex == lvStage.EditIndex)
                {
                    DropDownList ddlPrj = (DropDownList)e.Item.FindControl("ddlPrjNameEdit"); if (ddlPrj != null)
                    {
                        string lsPrjCode = ((Label)e.Item.FindControl("lblPrjCodeEdit")).Text;
                        LoadPrj(lsPrjCode);
                        ddlPrj.DataSource = dsPrjList.Tables[0];
                        ddlPrj.DataTextField = "PrjName";
                        ddlPrj.DataValueField = "PrjCode";
                        ddlPrj.DataBind();
                        if (!lsPrjCode.Equals("")) ddlPrj.SelectedValue = lsPrjCode;
                    }

                    DropDownList ddlUser = (DropDownList)e.Item.FindControl("ddlUserNameEdit"); if (ddlUser != null)
                    {
                        string lsUserID = ((Label)e.Item.FindControl("lblUserIDEdit")).Text;
                        ddlUser.DataSource = dsUserList.Tables[0];
                        ddlUser.DataTextField = "UserName";
                        ddlUser.DataValueField = "UserID";
                        ddlUser.DataBind();
                        if (!lsUserID.Equals("")) ddlUser.SelectedValue = lsUserID;
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

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            string lsPrjCode = string.Empty, lsUserID = string.Empty; 
            int iNo = ds.Tables[0].Rows.Count + 1;

            LoadPrj("");

            if (dsPrjList != null && dsPrjList.Tables != null && dsPrjList.Tables[0].Rows.Count > 0)
            {
                lsPrjCode = dsPrjList.Tables[0].Rows[0]["PrjCode"].ToString();
            }
            else
            {
                // Show message
                //MessageBoxShow("All projects had Project Manager. Cannot add new PM for project.");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('All projects had Project Manager. Cannot add new PM for project.','');", true);
                return;
            }
            if (dsUserList != null && dsUserList.Tables != null && dsUserList.Tables[0].Rows.Count > 0)
            {
                lsUserID = dsUserList.Tables[0].Rows[0]["UserID"].ToString();
            }

            ds.Tables[0].Rows.Add("0", lsUserID, lsPrjCode, 1);
            this.lvStage.EditIndex = iNo - 1;
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
            this.lvStage.EditIndex = iNo - 1;
            btnAddRecord.Enabled = false;
            
        }

        private void Save(string asID, ListViewItem lvi)
        {
            if (asID.Length < 1) return;

            string lsPrjCode = ((DropDownList)lvi.FindControl("ddlPrjNameEdit")).SelectedItem.Value;
            string lsUserID = ((DropDownList)lvi.FindControl("ddlUserNameEdit")).SelectedItem.Value;
            string lsStatus = ((DropDownList)lvi.FindControl("ddlStatusEdit")).SelectedItem.Value;

            string ls_SqlCommand = string.Empty;
            if (asID.Equals("0")) // Insert
            {
                ls_SqlCommand = " Insert Into tbl_ProjectManager (UserID, PrjCode, [Status]) "
                   + " Values('" + lsUserID + "', '" + lsPrjCode + "', " + lsStatus + " )";
            }
            else // Update
            {
                ls_SqlCommand = "Update tbl_ProjectManager Set UserID = '" + lsUserID + "', PrjCode = '" + lsPrjCode + "', [Status] = " + lsStatus + " Where [ID] = " + asID;
            }

            if (!ls_SqlCommand.Equals("")) SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, ls_SqlCommand);

            Response.Redirect(Request.RawUrl);
        }

        protected void ddlPrjName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {

        }

        protected void ddlUserFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        //private void MessageBoxShow(string message)
        //{
        //    lblMsgDesc.Text = message;
        //    lblMsgDesc.Visible = true;
        //    t1.Enabled = true;
        //    mi_counter = 0;
        //}

        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    mi_counter++;
        //    if (mi_counter == 3)
        //    {
        //        mi_counter = 0;
        //        lblMsgDesc.Visible = false;
        //        t1.Enabled = false;
        //    }
        //}
    }
}