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
    public partial class Popup_CreatePeriod : System.Web.UI.Page
    {
        protected static DataSet warehousesItems;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddl_Month.SelectedValue = DateTime.Now.ToString("MM");
                BuiltYear();
                txtNumberOfPeriod.Text = "12";
            }
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNumberOfPeriod.Text.Trim()))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("StartDate", ddl_Month.SelectedValue + "/01/" + ddl_Year.SelectedValue );
                dic.Add("NoP", txtNumberOfPeriod.Text);
                
                Session["chosenCreatePeriod"] = dic;
                Session["chosenItemNo"] = Request.QueryString["id"];
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OKWareHousePopup", "Main.okDialogClick('CreatePeriodCallBack');", true);
        }

        protected void BuiltYear()
        {
            if (ddl_Year.Items.Count > 0) ddl_Year.Items.Clear();
            for (int i = 0; i < DateTime.Now.Year - 2013 + 3; i++)
            {
                ListItem item = new ListItem();
                item.Text = (2013 + i).ToString();
                item.Value = (2013 + i).ToString();
                ddl_Year.Items.Add(item);
            }
            ddl_Year.SelectedValue = DateTime.Now.Year.ToString();
        }
    }
}