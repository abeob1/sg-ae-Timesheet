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
    public partial class SubcontractorExpenseEntry : System.Web.UI.Page
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

            ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.StoredProcedure, "sp_SubcontExpenseEntry", Data.CreateParameter("@IN_BeginDate", BeginDate.ToString("MM/dd/yyyy")),
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
                        SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.Text, "Delete tbl_SubcontractorExpense Where internal_id = '" + lsID + "'");
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
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            Project chosenProject = Session["chosenProject"] as Project;
                            DataRow dr = ds.Tables[0].Rows[itemNo - 1];
                            if (chosenProject != null)
                            {
                                //string[] lsArr = chosenProject.PrjCode.Split(';');
                                dr["PrjCode"] = chosenProject.PrjCode.Split(';')[0];
                                dr["SAPB1DB"] = chosenProject.PrjCode.Split(';')[1];
                                this.lvStage.DataSource = dv;
                                this.lvStage.EditIndex = ds.Tables[0].Rows.Count - 1;
                                this.lvStage.DataBind();
                            }

                            //itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            //if (chosenItem != null)
                            //{
                            //    AddUpdateItem(itemNo, chosenItem);
                            //    this.lvStage.DataSource = dv;
                            //    this.lvStage.EditIndex = ds.Tables[0].Rows.Count - 1;
                            //    this.lvStage.DataBind();
                            //}
                            break;

                        case "ConsultantFilterCallBack":
                            UserList chosenUser = Session["chosenUser"] as UserList;
                            itemNo = Int32.Parse(Session["chosenItemNo"] as String);
                            if (chosenUser != null)
                            {
                                AddUpdateEmp(itemNo, chosenUser);
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
                string lsEmpCode = ((Label)lvi.FindControl("lblEmpCodeEdit")).Text;
                string lsPrjCode = ((Label)lvi.FindControl("lblPrjCodeEdit")).Text;
                string lsAmount = ((TextBox)lvi.FindControl("txtAmountEdit")).Text;
                string lsSAPB1DB = ((Label)lvi.FindControl("lblSAPB1DBEdit")).Text;

                if (string.IsNullOrEmpty(lsPrjCode) || string.IsNullOrEmpty(lsEmpCode) || string.IsNullOrEmpty(lsDate)) 
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors", "Main.setMasterMessage('The Date, Employee Code or Project Code cannot be empty.','');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading", "Dialog.hideLoader();", true);
                    return;
                }
                SqlHelper.ExecuteNonQuery(Data.ConnectionString, CommandType.StoredProcedure, "sp_InsUpdateSubcontractorExpense",
                    Data.CreateParameter("@IN_InternalID", lsID),
                    Data.CreateParameter("@IN_Date", DateTime.Parse(lsDate)),
                    Data.CreateParameter("@IN_UserCode", lsEmpCode),
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
            Response.Redirect("/TimeSheet/SubcontractorExpenseEntry.aspx?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text);
        }

        #region AddUpdate Employee
        private void AddUpdateEmp(Int32 itemNo, UserList chosenUser)
        {
            if (chosenUser != null)
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

                //string[] lsArr = chosenUser.PrjCode.Split(';');
                //int iNo = ds.Tables[0].Rows.Count + 1;

                drX["No"] = CurNo.ToString();
                drX["Date"] = DateTime.Now.ToString("MM/dd/yyyy");
                drX["UserCode"] = chosenUser.UserName;
                //drX["PrjCode"] = lsArr[0];
                drX["Amount"] = 0;
                //drX["SAPB1DB"] = lsArr[1];
               
                if (itemNo == 0) ds.Tables[0].Rows.Add(drX);
            }
        }
        #endregion

    }
}