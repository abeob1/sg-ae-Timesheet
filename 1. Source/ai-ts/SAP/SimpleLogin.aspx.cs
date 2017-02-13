using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAP
{
    public partial class SimpleLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                //if (Request.Browser.Browser == "IE")
                //{
                //    this.Login1.Enabled = false;
                //    return;
                //}
                //if (User.Identity.IsAuthenticated == true)
                //{
                // //   Response.Redirect("Homepage.aspx");
                //    Response.Redirect("~/TimeSheet/WeekendTimeSheet.aspx");
                //}
            }
            
        }

      
    }
}
