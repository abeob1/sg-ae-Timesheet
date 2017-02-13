using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAP
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                //            "javascript:Main.openDialog(\"../Popup_EditItem.aspx\",id=\"" + "1" + "\")", true);

                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowLoader", "Dialog.showLoader();", true);

                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                //        "Main.setMasterMessage('Connecting to SAP.....','');", true);

                //GetDefault df = new GetDefault();
                //df.GetConnection(HttpContext.Current.User.Identity.Name);

                //if (df.GetConnection(HttpContext.Current.User.Identity.Name) == -1)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                //        "Main.setMasterMessage('Connect to SAP Failed!','');", true);
                //}
                ////else
                ////{
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OKErrors",
                //        "Main.setMasterMessage('Creating UDFs.....','');", true);

                //df.CreateUDF(HttpContext.Current.User.Identity.Name);
                //}

                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading",
                //                                        "Dialog.hideLoader();", true);
            }
            catch(Exception)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseLoading",
                                                        "Dialog.hideLoader();", true);
            }
           
        }
    }
}