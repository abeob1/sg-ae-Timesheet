using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAP.Admin.DAO;
using SAP.Admin;
using System.Collections;


namespace SAP
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckIsPM() == false)
                {
                    PendingApproval.Visible = false;
                    mnreport.Visible = false;
                    // Added by thangnv on 20131202
                    PendingApprovalByPeriod.Visible = false;
                }
                else
                {
                    PendingApproval.Visible = true;
                    mnreport.Visible = true;
                    // Added by thangnv on 20131202
                    PendingApprovalByPeriod.Visible = true;
                }

                switch (GetRoleName())
                {
                    case "Administrator":
                        adminmenu.Visible = true;
                        mnreport.Visible = true;
                        break;

                    case "ReportingManager":
                        adminmenu.Visible = false;
                        mnreport.Visible = true;
                        break;

                    case "Sub-contractor":
                        mnreport.Visible = false;
                        adminmenu.Visible = false;
                        //PendingApproval.Visible = false;
                        //PendingApprovalByPeriod.Visible = false;
                        ResourcesPlanning.Visible = false;
                        ProjectMandaysUsedReport.Visible = false;
                        ProjectMandaysUsedByPhaseReport.Visible = false;
                        ChangePassword.Visible = false;
                        break;

                    default :
                        mnreport.Visible = false;
                        adminmenu.Visible = false;
                        break;
                }


                /*if (CheckIsRoleAdmin())
                {
                    adminmenu.Visible = true;
                    mnreport.Visible = true;
                }
                else if (CheckIsRoleReportingManager())
                {
                    adminmenu.Visible = false;
                    mnreport.Visible = true;
                }
                else
                {
                    mnreport.Visible = false;
                    adminmenu.Visible = false;
                }*/

                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowLoader", "Dialog.showLoader();", true);
                //MasterData masterDataWS = new MasterData();
                //lblCompany.Text = masterDataWS.GetCompanySetting(HttpContext.Current.User.Identity.Name).Tables[0].Rows[0]["CompanyName"].ToString();
               // AuthorizeUser();
            }
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            //GetDefault df = new GetDefault();
            //df.LogOut("");
            Constants.SessionKeys.My_Session = string.Empty;
        }

        private void AuthorizeUser()
        {
            string pageName = Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower().Trim().Replace(".aspx", "");
            if (pageName.StartsWith("login") || pageName.StartsWith("default") || pageName.StartsWith("home") || pageName.StartsWith("exception") || pageName.StartsWith("Main"))
            {
                return;
            }

            string userName = "";

            if (Context.User != null)
            {
                userName = Context.User.Identity.Name.ToLower();
            }

            if (!AuthorizeUser(userName, pageName))
            {
                Response.Redirect("~/PermissionDenied.aspx");
            }
        }

        public static bool AuthorizeUser(string userName, string pageName)
        {
            try
            {
                /*if (userName.ToLower().Trim() == "admin")
                {

                    return true;
                }*/

                if (pageName.Contains("login") || pageName.Contains("permissiondenied"))
                    return true;

                string[] roleName = Roles.GetRolesForUser(userName);
                if (roleName == null || roleName.Length == 0) { return false; }
                RolePermissions roleController = new RolePermissions();
                List<RolePermissions> allowedPages = null;


                if (HttpContext.Current.Session["AllowedPages"] != null)
                {
                    allowedPages = (List<RolePermissions>)HttpContext.Current.Session["AllowedPages"];
                }
                else
                {
                    allowedPages = new List<RolePermissions>();
                    for (int i = 0; i < roleName.Length; i++)
                    {
                        List<RolePermissions> allowedPage = roleController.GetByRolePermissionName(roleName[i]);
                        if (allowedPage != null || allowedPage.Count > 0)
                        {
                            allowedPages.AddRange(allowedPage);
                        }
                    }
                    HttpContext.Current.Session.Add("AllowedPages", allowedPages);
                    Constants.SessionKeys.My_Session = Guid.NewGuid().ToString();
                }

                if (allowedPages != null && allowedPages.Count > 0)
                {
                    foreach (RolePermissions allowedPage in allowedPages)
                    {
                        if (allowedPage.PageName.ToLower().Trim().Contains(pageName))
                        {
                            return (bool)allowedPage.Accessable;
                        }
                    }
                    return false;
                }
                

                return (bool)allowedPages[0].Accessable;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Boolean CheckIsPM()
        {
            DataSet ds = SqlHelper.ExecuteDataSet(Data.ConnectionString, CommandType.Text,
                "select * from tbl_ProjectManager T0 join aspnet_Users T1 on T0.UserID=T1.UserId where UserName='" + HttpContext.Current.User.Identity.Name + "'");
            if ( (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0) ) return false;
            return true;
        }

        /*
        private bool CheckIsRoleAdmin()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            if (roles == null || roles.Length == 0) return false;

            List<string> ar = roles.ToList<string>();
            return ar.Contains("Administrator");
        }

        private bool CheckIsRoleReportingManager()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            if (roles == null || roles.Length == 0) return false;

            List<string> ar = roles.ToList<string>();
            return ar.Contains("ReportingManager");
        }

        private bool CheckIsRoleSubcontractor()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            if (roles == null || roles.Length == 0) return false;

            List<string> ar = roles.ToList<string>();
            return ar.Contains("Sub-contractor");
        }*/

        private string GetRoleName()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);

            if (roles == null || roles.Length == 0) return "";
            List<string> ar = roles.ToList<string>();

            if (ar.Contains("Administrator"))
                return "Administrator";
            else if (ar.Contains("ReportingManager"))
                return "ReportingManager";
            else if (ar.Contains("Sub-contractor"))
                return "Sub-contractor";

            return "";
        }
 
    }
}