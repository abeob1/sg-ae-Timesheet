using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SAP.Admin
{
    public class oUsers
    {
        public string CreateUser(string userName, string password, string roleName, string email)
        {
            try
            {

                MembershipUser user = Membership.GetUser(userName);

                //Add new user
                if (user == null)
                {

                    if (string.IsNullOrEmpty(email))
                    {
                        WebHelper.AddMemberShipUserWithoutEmail(userName, password, roleName);
                    }
                    else
                    {
                        WebHelper.AddMemberShipUser(userName, password, email, roleName);
                    }

                    user = Membership.GetUser(userName);
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                }
                
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}