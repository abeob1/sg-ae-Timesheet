using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
//using Pyco.Framework;
using System.Collections.Generic;


namespace SAP.Admin
{
    public class WebHelper
    {
        public const string Unauthorization = "9999";
        public const string InvalidAuthentication = "9989";
        public const string BlackListedNumber = "9979";
        public const string InvalidParameter = "9969";
        public const string InvalidMobilephoneNumber = "9959";
        public const string UnauthorizationOrExceedSMSLimit = "9949";
        public const string InvalidShortCode = "9939";
        public const string SendSMSFailed = "9929";
        public const string UsageLimitChargeReached = "9919";
        public const string InvalidMessageLength = "9909";
        public const string InvalidServiceType = "9900";
        public const string RoutingInfoListedNumber = "8999";
        public const string UnsupportedCarrier = "8888";
        public const string ForbiddenCharacter = "!,@,#,$,%,^,&,*,(,)";

        public static string[] ResponseDescription = { 
                                            "Unauthorization", 
                                            "InvalidAuthentication",
                                            "BlackListedNumber",
                                            "InvalidParameter",
                                            "Invalid MobilephoneNumber",
                                            "Unauthorization Or ExceedSMSLimit",
                                            "Invalid ShortCode",
                                            "SendSMS Failed",
                                            "UsageLimit Charge Reached",
                                            "InvalidMessageLength",
                                            "Invalid ServiceType",
                                            "RoutingInfo ListedNumber",
                                            "UnsupportedCarrier",    
                                            "Sent successfully"
                                        };

        public static string[] ResponseCodes = { "9999", 
                                                "9989", 
                                                "9979", 
                                                "9969", 
                                                "9959", 
                                                "9949", 
                                                "9939", 
                                                "9929", 
                                                "9919", 
                                                "9909", 
                                                "9900",
                                                "8999", 
                                                "8888", 
                                                "1" };
        public const string VIEWSTATE_ORDER_BY = "orderBy";
        public const string VIEWSTATE_ORDER_DIRECTION = "orderDirection";
        public const int BRIEF_SIZE = 30;

        public static Regex EXCEPTION_TRACE_DELIM_REGEX = new Regex(@"[\r\n ]+at ");
        public static string EXCEPTION_NAME_PREFIX_HTML = "<strong>";
        public static string EXCEPTION_NAME_SUFFIX_HTML = "</strong>";
        public static string EXCEPTION_TRACE_PREFIX_HTML = "   at ";
        public static string EXCEPTION_TRACE_SUFFIX_HTML = "";
        public static string EXCEPTION_HTML_DELIM = "<hr>";
        public static string EXCEPTION_TRACE_HTML_DELIM = "<br>";
        public static Regex EMPTY_STRING_REGEX = new Regex(@"^[ \t]*$");
        public const string NEED_CHANGE_PASSWORD_INDICATOR = "Need Change Password";
        public const string PASSWORD_CHANGED_INDICATOR = "Password Changed";

        public static string AppPath
        {
            get
            {
                string path = HttpContext.Current.Request.ApplicationPath;
                if (!path.EndsWith("/")) path = path + "/";
                return path;
            }
        }


        public static bool IsExistSpecialCharacter(String name)
        {
            try
            {
                ArrayList forbiddenList = new ArrayList(ForbiddenCharacter.Split(new char[] { ',' }));
                foreach (string ch in forbiddenList)
                {
                    if (name.IndexOf(ch) >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        public static MembershipUser AddMemberShipUser(string userName, string password, string email, string roleName)
        {
            MembershipUser user = Membership.CreateUser(userName, password, email);
            if (user == null)
            {
                Logger.Error("Error occurs when adding new user");
                return null;
            }

            // Update status for change password
            user.Comment = NEED_CHANGE_PASSWORD_INDICATOR;
            Membership.UpdateUser(user);

            Roles.AddUserToRole(userName, roleName);

            return user;
        }

        public static void AddMemberShipUserWithoutEmail(string userName, string password, string roleName)
        {
            MembershipUser user = Membership.CreateUser(userName, password);
            Roles.AddUserToRole(userName, roleName);
        }

        public static void DeleteMembershipUser(string userName)
        {
            try
            {
                RemoveUserFromRoles(userName);
                Membership.DeleteUser(userName, true);
            }
            catch (Exception ex)
            {
                Logger.Error("DeleteMembershipUser" + ex.Message);
            }
        }

        public static void RemoveUserFromRoles(string userName)
        {
            string[] roles = Roles.GetAllRoles();
            foreach (string role in roles)
            {
                if (!Roles.IsUserInRole(userName, role)) continue;
                Roles.RemoveUserFromRole(userName, role);
            }
        }

        public static bool IncludeInStringList(string data, string[] listItem)
        {
            if (listItem == null || listItem.Length == 0)
                return false;

            foreach (string item in listItem)
            {
                if (item.Trim().ToLower() == data.Trim().ToLower())
                    return true;
            }

            return false;
        }

        #region Current User
        public static Guid GetCurrentUserID()
        {
            return GetUserID(GetCurrentUserName());
            //return new Guid(Membership.GetUser(GetCurrentUserName()).ProviderUserKey.ToString());
        }

        public static Guid GetUserID(string userName)
        {
            if (string.IsNullOrEmpty(userName)) { return new Guid(); }

            MembershipUser user = Membership.GetUser(userName);
            if (user != null)
            {
                return new Guid(user.ProviderUserKey.ToString());
            }

            return new Guid();
        }

        public static string GetUserName(Guid? userId)
        {
            if (userId.HasValue)
            {
                MembershipUser user = Membership.GetUser(userId, false);
                if (user != null)
                {
                    return user.UserName;
                }
            }

            return string.Empty;
        }

        public static string GetCurrentUserName()
        {
            if (System.Web.HttpContext.Current.User == null)
                return "";
            if (System.Web.HttpContext.Current.User.Identity != null)
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
            }

            if (System.Web.HttpContext.Current.Items["CurrentUser"] == null)
                return "";
            return System.Web.HttpContext.Current.Items["CurrentUser"].ToString();
        }

        public static string GetCurrentUserRole()
        {
            string userName = GetCurrentUserName();
            string[] rolesOfUser = Roles.GetRolesForUser(userName);

            if (rolesOfUser == null || rolesOfUser.Length == 0) { return string.Empty; }
            return rolesOfUser[0];
        }

        
        public static bool IsRoleForUser(string userName, string roles)
        {
            try
            {
                string[] rolesOfUser = Roles.GetRolesForUser(userName);

                if (rolesOfUser == null || rolesOfUser.Length == 0)
                    return false;

                string[] rolesList = roles.Split(',');

                if (rolesList == null || rolesList.Length == 0)
                    return false;

                foreach (string roleOfUser in rolesOfUser)
                    foreach (string role in rolesList)
                    {
                        if (roleOfUser.Trim().ToUpper() == role.Trim().ToUpper())
                            return true;
                    }
            }
            catch (Exception ex)
            {
                Logger.Error("IsRoleForUser exception: " + ex);
            }
            return false;
        }

        #endregion
        public static string LanguageCultureCookieName
        {
            get
            {
                return FormsAuthentication.FormsCookieName + "_YP_Accumulate_Language_Culture";
            }
        }


        public static string GetSiteRoot
        {
            get
            {
                string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
                if (port == null || port == "80" || port == "443")
                    port = "";
                else
                    port = ":" + port;

                string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
                if (protocol == null || protocol == "0")
                    protocol = "http://";
                else
                    protocol = "https://";

                string sOut = protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port +
                              HttpContext.Current.Request.ApplicationPath;

                if (sOut.EndsWith("/"))
                {
                    sOut = sOut.Substring(0, sOut.Length - 1);
                }

                return sOut;
            }
        }
    }
}
