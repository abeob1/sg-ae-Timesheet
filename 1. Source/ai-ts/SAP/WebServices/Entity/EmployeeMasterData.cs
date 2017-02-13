using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class EmployeeMasterData
    {
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
        private string _MidName;

        public string MidName
        {
            get { return _MidName; }
            set { _MidName = value; }
        }
        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        public static List<EmployeeMasterData> extractFromDataSet(DataTable table) 
        {
            List<EmployeeMasterData> list = new List<EmployeeMasterData>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    EmployeeMasterData data = new EmployeeMasterData();
                    data.Code = row[0].ToString();
                    data.FirstName = row[1].ToString();
                    data.MidName = row[2].ToString();
                    data.LastName = row[3].ToString();
                    list.Add(data);
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