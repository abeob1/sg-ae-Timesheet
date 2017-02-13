using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class ContactPerson
    {
        private string _IsDefault;

        public string IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }
        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }


       
        public static List<ContactPerson> extractFromDataSet(DataTable table) 
        {
            List<ContactPerson> list = new List<ContactPerson>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    ContactPerson person = new ContactPerson();
                    person.IsDefault = row[3].ToString();
                    person.Code = row[1].ToString();
                    person.FirstName = row[2].ToString();
                    person.LastName = row[0].ToString();
                    list.Add(person);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return list;
        }
    }
    
}