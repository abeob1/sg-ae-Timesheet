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
using SAP.Admin;

namespace SAP
{
    public partial class TimesheetPosting : System.Web.UI.Page
    {
        #region ds
        protected DataSet ds
        {
            get { return (DataSet)ViewState["ds"]; }
            set { ViewState["ds"] = value; }
        }
        #endregion

        #region Event 

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ds = new DataSet();
                BuiltYear();

                if (Request.QueryString["year"] != null && Request.QueryString["year"].Length == 4)
                {
                    ddl_Year.SelectedValue = Request.QueryString["year"];
                }
                else
                {
                    ddl_Year.SelectedValue = DateTime.Now.Year.ToString();
                }

                Binding();
            }
        }
        #endregion

        #region lvStage_ItemCommand
        protected void lvStage_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
            ListViewItem lvi = e.Item;
            Label lblID;
            switch (e.CommandName)
            {
                case "Update":
                    lblID = (Label)lvi.FindControl("lblIDEdit");
                    DropDownList ddlLock = (DropDownList)lvi.FindControl("ddl_StatusEdit");
                    SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Update tbl_TimsheetPostingPeriod Set Date_Locked = " + (ddlLock.SelectedValue == "1" ? "1" : "0")
                        + " Where ID = " + lblID.Text);
                    Response.Redirect("/TimeSheet/TimesheetPosting.aspx?year=" + ddl_Year.SelectedValue);
                    break;

                case "Edit":
                    btnAddRecord.Enabled = 
                    btnFilter.Enabled =
                    ddl_Year.Enabled = false;
                    break;

                case "Delete":
                    int li_ErrorCode = 0;
                    try
                    {
                        string lsID = dr["ID"].ToString();
                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Delete tbl_TimsheetPostingPeriod Where ID = " + lsID);
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
                        Response.Redirect("/TimeSheet/TimesheetPosting.aspx?year=" + ddl_Year.SelectedValue);
                    } break;

                case "Cancel":
                    Response.Redirect("/TimeSheet/TimesheetPosting.aspx?year=" + ddl_Year.SelectedValue);
                    break;
            }
        }
        #endregion

        #region lvStage_ItemEditing
        protected void lvStage_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            this.lvStage.EditIndex = e.NewEditIndex;
            ListViewItem item = this.lvStage.Items[e.NewEditIndex];
            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }
        #endregion

        #region lvStage_ItemDataBound
        protected void lvStage_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvStage.EditIndex < 0) return;
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (dataItem.DisplayIndex == lvStage.EditIndex)
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddl_StatusEdit"); if (ddl != null)
                {
                    DataRow dr = ds.Tables[0].Rows[e.Item.DataItemIndex];
                    ddl.SelectedIndex = (dr["Date_Locked"].ToString().Equals("Locked") ? 1 : 0);
                }
            }
        }
        #endregion

        #region lvStage_LayoutCreated
        protected void lvStage_LayoutCreated(object sender, EventArgs e)
        {

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
                    Int32 itemNo = 0;
                    DataView dv = new DataView(ds.Tables[0]);
                    switch (this.Request["__EVENTARGUMENT"].ToString())
                    {
                        case "CreatePeriodCallBack":
                            Dictionary<string, string> dic = Session["chosenCreatePeriod"] as Dictionary<string, string>;
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            if (dic != null && dic.Count > 0)
                            {
                                AddUpdateItem(itemNo, dic);
                            } break;

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
        #endregion

        #region btnFilter_Click
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TimeSheet/TimesheetPosting.aspx?year=" + ddl_Year.SelectedValue);
        }
        #endregion

        #endregion

        #region Methods

        #region Binding
        void Binding()
        {
            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text, " Select RIGHT(CONVERT(VARCHAR(9), Starting_Date, 6), 6) As [Month], "
             + " case when Date_Locked = 0 then 'Unlocked' else 'Locked' end As Date_Locked, ID "
             + " From dbo.tbl_TimsheetPostingPeriod Where Year(Starting_Date) = " + ddl_Year.SelectedValue
             + " Order By Starting_Date ");

            DataView dv = new DataView(ds.Tables[0]);
            this.lvStage.DataSource = dv;
            this.lvStage.DataBind();
        }
        #endregion

        #region AddUpdateItem
        private void AddUpdateItem(Int32 itemNo, Dictionary<string, string> dic)
        {
            if (dic != null)
            {
                int liNoP = int.Parse(dic["NoP"]);
                DateTime StartDate = Convert.ToDateTime(dic["StartDate"], new CultureInfo("es-US"));

                if (DataValidation(StartDate, liNoP))
                {
                    ArrayList sqlList = new ArrayList();
                    DatabaseParameters KeyParameters = new DatabaseParameters();

                    for (int i = 0; i < liNoP; i++)
                    {
                        DateTime dt = StartDate.AddMonths(i);
                        KeyParameters.Clear();
                        KeyParameters.Add(new DatabaseParameter("Starting_Date", dt.ToString("MM/dd/yyyy")));
                        KeyParameters.Add(new DatabaseParameter("Closed", "0"));
                        KeyParameters.Add(new DatabaseParameter("Date_Locked", "0"));
                        KeyParameters.Add(new DatabaseParameter("CreatedUser", User.Identity.Name));
                        KeyParameters.Add(new DatabaseParameter("CreatedDate", DateTime.Now.ToString("MM/dd/yyyy")));

                        SqlHelper.InsertCommand(KeyParameters, "tbl_TimsheetPostingPeriod");
                        sqlList.Add(SqlHelper.SQL);
                    }

                    if (!(SqlHelper.ExecuteQuery(sqlList, Data.ConnectionString)))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('" + SqlHelper.ErrMsg + "','');", true);
                    }
                    else
                    {
                        Response.Redirect("/TimeSheet/TimesheetPosting.aspx?year=" + ddl_Year.SelectedValue);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('Cannot Create Posting Period. It is existed','');", true);
                }
            }
        }
        #endregion

        #region DataValidation
        protected bool DataValidation(DateTime StartDate, int NoP)
        {
            DateTime EndDate = StartDate.AddMonths(NoP - 1);
            string lsSql = "Select count(*) From dbo.tbl_TimsheetPostingPeriod Where convert(varchar(6), Starting_Date, 112) >= convert(varchar(6), cast('" + StartDate.ToString("MM/dd/yyyy") + "' As date) , 112) and "
                + " convert(varchar(6), Starting_Date, 112) <= convert(varchar(6), cast('" + EndDate.ToString("MM/dd/yyyy") + "' As date), 112) ";
            // Check period existed or not.
            SqlDataReader reader = (SqlDataReader)SqlHelper.ExecuteReader(Data.ConnectionString, CommandType.Text, lsSql);
            reader.Read();
            return !(int.Parse(reader[0].ToString()) > 0);
        }
        #endregion

        protected void BuiltYear()
        {
            if (ddl_Year.Items.Count > 0) ddl_Year.Items.Clear();
            for(int i = 0; i < DateTime.Now.Year - 2013 + 3; i++)
            {
                ListItem item = new ListItem();
                item.Text = (2013 + i).ToString();
                item.Value = (2013 + i).ToString();
                ddl_Year.Items.Add(item);
            }
        }

        #endregion
    }
}