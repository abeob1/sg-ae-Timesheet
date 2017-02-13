using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAP.WebServices;
using SAP.Admin.DAO;

namespace SAP
{
    public partial class Popup_MoreInfo : System.Web.UI.Page
    {
        protected static DataSet warehousesItems;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["moreinfo"] != null && Request.QueryString["moreinfo"].Length > 0)
                {
                    string[] lsMoreInfo = Request.QueryString["moreinfo"].ToString().Split(';');
                    if (lsMoreInfo.Length > 0) txtCountry.Text = lsMoreInfo[0];
                    if (lsMoreInfo.Length > 1) txtEBMName.Text = lsMoreInfo[1];
                    if (lsMoreInfo.Length > 2) txtProspectName.Text = lsMoreInfo[2];
                }
                if (Request.QueryString["status"] != null && Request.QueryString["status"].Length > 0)
                {
                    string lsStatus = Request.QueryString["status"];
                    if (lsStatus.ToString().Equals("Editing") || lsStatus.ToString().Equals("Adding"))
                        btnAdd.Visible = true;
                    else
                        btnAdd.Visible = false;
                }
                else
                    btnAdd.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Session["chosenMoreInfo"] = txtCountry.Text.Trim() + ";" + txtEBMName.Text.Trim() + ";" + txtProspectName.Text.Trim();
            //Session["chosenItemNo"] = Request.QueryString["id"];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OKWareHousePopup", "Main.okDialogClick('MoreInfoCallBack');", true);
        }
    }
}