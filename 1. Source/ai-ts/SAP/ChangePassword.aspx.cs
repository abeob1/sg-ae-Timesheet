using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace SAP
{
    public partial class ChangePassword : System.Web.UI.Page
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
                //MembershipUser user = Membership.GetUser("veronicay", false);

                //string Pass = Decrypt("mPzsHre2wN5Nb+eR1jVw2grl7B8=");
                
                //string lsPassword = Server.HtmlDecode("mPzsHre2wN5Nb+eR1jVw2grl7B8=");
                
            }
            
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(),CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected void btn_Ok_Click(object sender, ImageClickEventArgs e)
        {
            String oldPass = Password.Text;
            String newPass = NewPassword.Text;
            String confPass = ConfirmPassword.Text;
            if (newPass != confPass) {
                FailureText.Text = "New password and confirm password do not match";
                return;
            }
            MembershipUser user = Membership.GetUser(User.Identity.Name, false);
            
            try
            {
                if (user.ChangePassword(oldPass, newPass))
                {
                    FailureText.Text = "Password changed";
                }
                else
                    FailureText.Text = "Password change failed.";
            }
            catch (Exception ex) {
                FailureText.Text = "Password change failed. " + ex.Message;
            }
            
            //1.1 ok then change
        }
      
    }
}
