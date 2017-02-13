using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class Territory
    {
        private int _territryID;

        public int territryID
        {
            get { return _territryID; }
            set { _territryID = value; }
        }
        private string _descript;

        public string descript
        {
            get { return _descript; }
            set { _descript = value; }
        }
        private string _Parent;

        public string Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }


        public static List<Territory> extractFromDataSet(DataTable table) 
        {
            List<Territory> list = new List<Territory>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    Territory data = new Territory();
                    data.territryID = int.Parse(row[0].ToString());
                    data.descript = row[1].ToString();
                    data.Parent = row[2].ToString();
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