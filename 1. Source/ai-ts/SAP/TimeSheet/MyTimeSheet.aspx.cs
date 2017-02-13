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
    public partial class MyTimeSheet : System.Web.UI.Page
    {
        private static DataSet ds = new DataSet();
        private static DataSet dsProject = new DataSet();
        private static int mi_idx = 0;
        private static string FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        private static string ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("MM/dd/yyyy");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProject();
                Binding();
            }
        }

        void Binding()
        {
            txtFromDate.Text = FromDate;
            txtToDate.Text = ToDate;

            ddl_Project.DataSource = dsProject.Tables[0];
            ddl_Project.DataTextField = "PrjName";
            ddl_Project.DataValueField = "PrjCode";
            ddl_Project.DataBind();
            ddl_Project.Items.Insert(0, "");
            ddl_Project.SelectedIndex = mi_idx;
            string lsPrjCode = string.Empty;
            string[] lsaProject = ddl_Project.SelectedValue.Split(';');
            if(lsaProject.Length > 0)
                lsPrjCode = lsaProject[0];
            else
                lsPrjCode = ddl_Project.SelectedValue;

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_MyTimeSheet",
                Data.CreateParameter("@IN_BeginDate", txtFromDate.Text), 
                Data.CreateParameter("@IN_EndDate", txtToDate.Text), 
                Data.CreateParameter("@IN_UserCode", User.Identity.Name),
                Data.CreateParameter("@IN_PrjCode", lsPrjCode),
                Data.CreateParameter("@IN_Status", ddlType.SelectedIndex.ToString()));

            if (ddlType.SelectedIndex < 0) ddlType.SelectedIndex = 0;
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }

        private void LoadProject()
        {
            dsProject = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
        }

        protected void ProductListPagerCombo_PreRender(object sender, EventArgs e)
        {
            //Binding();
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            if (e.CommandName == "Edit")
            {
                string ls_RetVal = string.Empty;
                //ListViewItem lvi = e.Item;
                ls_RetVal = dr["ID"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["Date"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["Hour"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["PrjCode"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["PrjName"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["Billable"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["Description"].ToString();
                ls_RetVal = ls_RetVal + ";" + dr["SAPB1DB"].ToString();
                //LinkButton lb = (LinkButton)e.CommandSource;
                Response.Redirect("~/TimeSheet/TimeEntry.aspx?ID=" + ls_RetVal);
            }
            else if (e.CommandName == "Delete")
            {
                int li_ErrorCode = 0;
                try
                {
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Update tbl_TimeEntry Set [Status] = 1 Where ID = '" + dr["ID"].ToString() + "'");
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
                    FromDate = txtFromDate.Text.Trim();
                    ToDate = txtToDate.Text.Trim();
                    Response.Redirect(Request.RawUrl);
                }

            }
        }

        protected void lvAll_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //if (e.CommandName == "View")
            //{
            //    LinkButton lb = (LinkButton)e.CommandSource;
            //    Response.Redirect("/TimeSheet/ABEO_TIMESHEET.aspx?clgCode=" + lb.CommandArgument);
            //}
        }

        protected void lvStage_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            //this.lvStage.EditIndex = e.NewEditIndex;
            //DataView dv = new DataView(ds.Tables[0]);
            //this.lvStage.DataSource = dv;
            //this.lvStage.DataBind();
        }

        protected void btnNewEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("TimeEntry.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FromDate = txtFromDate.Text.Trim();
            ToDate = txtToDate.Text.Trim();
            mi_idx = ddl_Project.SelectedIndex;
            Binding();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            SetForm();
        }

        private void SetForm()
        {
            for(int i = 0; i < lvStage.Items.Count ; i++)
            {
                ListViewItem lvi = lvStage.Items[i];
                if (ds.Tables[0].Rows[i]["PrjCode"].ToString().Equals("ZZZZZZZZZZZZZZZZZZZZ"))
                {
                    lvi.Controls.Remove((LinkButton)lvi.FindControl("imgbEdit"));
                    lvi.Controls.Remove((LinkButton)lvi.FindControl("imgbDelete"));
                    int idx = lvi.Controls.IndexOf((Label)lvi.FindControl("Date"));
                    lvi.Controls.Remove((Label)lvi.FindControl("Date"));
                    ((Label)lvi.FindControl("Hour")).Font.Bold = true;
                    Label lblTotal = new Label();
                    lblTotal.ID = "lblTotal"+ i.ToString();
                    if (i == lvStage.Items.Count - 1)
                    {
                        lblTotal.Text = "Grand Total:";
                        lblTotal.Font.Bold = true;
                        lvi.Controls.AddAt(idx, lblTotal);
                        ((Label)lvi.FindControl("PrjCode")).Text = string.Empty;
                    }
                    //else
                    //    lblTotal.Text = "Total:";
                    //lblTotal.Font.Bold = true;
                    //lvi.Controls.AddAt(idx, lblTotal);
                }
            }
        }
    }
}