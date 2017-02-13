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
using SAP.Admin.DAO;
using System.Globalization;

namespace SAP
{
    public partial class TimeEntry : System.Web.UI.Page
    {
        private static DataSet ds = new DataSet();
        private static string[] ms_UrlArr = null;
        private static int mi_counter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                if (ddlPrijCode.Items.Count <= 0)
                {
                    ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_ProjectList");
                    ddlPrijCode.DataSource = ds.Tables[0];
                    ddlPrijCode.DataBind();
                    ddlPrijCode.DataTextField = "PrjName";
                    ddlPrijCode.DataValueField = "PrjCode";
                    ddlPrijCode.DataBind();
                    if (ddlPrijCode.SelectedValue != null)
                    {
                        string[] prjArr = ddlPrijCode.SelectedValue.Split(';');
                        if(prjArr.Length > 0) txtPrjCode.Text = prjArr[0];
                    }
                }
                string lsUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                ParseData(lsUrl);
            }
        }

        private void ParseData(string currentUrl)
        {
            string Url = currentUrl.Replace("%20", " ");
            string[] temp = Url.Split('=');
            if(temp.Length > 1)
            {
                ms_UrlArr = temp[1].Split(';');
                txtDate.Text = ms_UrlArr[1].Substring(0, 10); ;
                txtHour.Text = ms_UrlArr[2];
                ddlPrijCode.SelectedValue = ms_UrlArr[3] + ";" + ms_UrlArr[7];
                ddlPrijCode.Text = ms_UrlArr[4];
                rblBill.Items[0].Selected = Convert.ToBoolean(ms_UrlArr[5]);
                txtDescription.Text = ms_UrlArr[6];
                txtPrjCode.Text = ms_UrlArr[3];
            }
        }

        void Binding()
        {

        }

        protected void ProductListPagerCombo_PreRender(object sender, EventArgs e)
        {
            //Binding();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ms_UrlArr = null;
            ClearScreen();
        }

        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //if (e.CommandName == "View")
            //{
            //    LinkButton lb = (LinkButton)e.CommandSource;
            //    Response.Redirect("/TimeSheet/ABEO_TIMESHEET.aspx?clgCode=" + lb.CommandArgument);
            //}
        }

        protected void lvAll_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //if (e.CommandName == "View")
            //{
            //    LinkButton lb = (LinkButton)e.CommandSource;
            //    Response.Redirect("/TimeSheet/ABEO_TIMESHEET.aspx?clgCode=" + lb.CommandArgument);
            //}
        }

        protected void btnPostEntry_Click(object sender, EventArgs e)
        {
            try
            {
                //if (HttpUtility.HtmlAttributeEncode(txtUserCode.Text.Trim()) == "")
                //{
                //    Session["errorMessage"] = "User code can't be blank";
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('User code can not be blank','');", true);
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                //    return;
                //}
                if (HttpUtility.HtmlAttributeEncode(txtHour.Text.Trim()).Equals(""))
                {
                    MessageBoxShow("Please input hour number!");
                    txtHour.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        if (decimal.Parse(HttpUtility.HtmlAttributeEncode(txtHour.Text.Trim())) <= 0)
                        {
                            MessageBoxShow("Hour must be more than zero.");
                            txtHour.Focus();
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBoxShow("Hour must be numeric.");
                        txtHour.Focus();
                        return;
                    }
                }
                
                int li_ErrorCode = 0;
                try
                {
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, _collectData());
                }
                catch (SqlException sqlEx)
                {
                    li_ErrorCode = sqlEx.ErrorCode;
                    lblMsgDesc.Text = "Error code: " + sqlEx.ErrorCode.ToString() + ". Error message: " + sqlEx.Message;
                    lblMsgDesc.Visible = true;
                }

                if (li_ErrorCode != 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + li_ErrorCode.ToString() + "','');", true);
                }
                else
                {
                    //Session["successMessage"] = "Operation complete sucessful!";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + "Operation complete sucessful!" + "','');", true);
                    MessageBoxShow("Operation complete sucessful!");                    
                    ms_UrlArr = null;
                    ClearScreen();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading",
                                                    "Dialog.hideLoader();", true);
            }
            catch (Exception ex)
            {
                //Session["errorMessage"] = ex.ToString();
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + ex.ToString() + "','');", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                MessageBoxShow("Error: " + ex.Message);
            }
        }

        private void ClearScreen()
        {
            txtHour.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtHour.Focus();
        }

        private string _collectData()
        {
            string ls_SqlCommand = string.Empty;
            CultureInfo ivC = new CultureInfo("es-US");
            string lsPrjCode = string.Empty, lsPrjSAPB1DB = string.Empty;
            string lsPrjName = ddlPrijCode.SelectedItem.Text;
            string[] prjArr = ddlPrijCode.SelectedValue.Split(';');
            if (prjArr.Length > 0)
            {
                lsPrjCode = prjArr[0];
                lsPrjSAPB1DB = prjArr[1];
            }

            if (ms_UrlArr == null)
            {
                ls_SqlCommand = " Insert Into tbl_TimeEntry(UserCode, [Date], [Hour], PrjCode, PrjName, Billable, [Description], [Status], SAPB1DB ) "
                + " Values('" + User.Identity.Name + "', '" + Convert.ToDateTime(txtDate.Text, ivC).ToString("yyyyMMdd") + "', " + txtHour.Text.Trim() 
                + ", '" + lsPrjCode + "', '" + lsPrjName + "', '" + rblBill.Items[0].Selected.ToString()
                + "' , '" + txtDescription.Text.Trim() + "', 0, '" + lsPrjSAPB1DB + "' )";
            }
            else
            {
                ls_SqlCommand = "Update tbl_TimeEntry Set [Date] = '" + Convert.ToDateTime(txtDate.Text, ivC).ToString("yyyyMMdd")
                + "' , [Hour] = " + txtHour.Text.Trim()
                + ", PrjCode = '" + lsPrjCode
                + "', PrjName = '" + lsPrjName
                + "', Billable='" + rblBill.Items[0].Selected.ToString()
                + "', [Description]= '" + txtDescription.Text.Trim() + "' Where [ID] = " + ms_UrlArr[0];
            }

            return ls_SqlCommand;
        }

        protected void btnMyEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TimeSheet/MyTimeSheet.aspx");
        }

        private void MessageBoxShow(string message)
        {
            lblMsgDesc.Text = message;
            lblMsgDesc.Visible = true;
            t1.Enabled = true;
            mi_counter = 0;
            //timer.Tick += new EventHandler(OnTimerEvent);
            //lblMsgDesc.Visible = false;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            mi_counter++;
            if(mi_counter==3)
            {
                mi_counter = 0;
                lblMsgDesc.Visible = false;
                t1.Enabled=false;
            }
            txtHour.Focus();
        }

        protected void ddlPrijCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prj = ddlPrijCode.SelectedValue;
            string[] prjArr = prj.Split(';');

            if (prjArr.Length > 0)
            {
                txtPrjCode.Text = prjArr[0];
            }
            else
            { txtPrjCode.Text = string.Empty; }
        }

        protected void btnWeekEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TimeSheet/WeekendTimeSheet.aspx");

        }
    }
}