using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Profile;

namespace SAP.Admin
{
    public class ProfileCommon
    {
        public String Email
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Email");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Email", value);
            }
        }

        public String Phone
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Phone");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Phone", value);
            }
        }

        public String Ref1
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Ref1");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Ref1", value);
            }
        }

        public String Ref2
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Ref2");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Ref2", value);
            }
        }

        public String Ref3
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Ref3");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Ref3", value);
            }
        }

        public String Ref4
        {
            get
            {
                return (String)HttpContext.Current.Profile.GetPropertyValue("Ref4");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("Ref4", value);
            }
        }

       
    }
}