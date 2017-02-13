using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class WareHouse
    {
        private string whsCode;

        public string WhsCode
        {
            get { return whsCode; }
            set { whsCode = value; }
        }

        private string whsName;

        public string WhsName
        {
            get { return whsName; }
            set { whsName = value; }
        }

        public static List<WareHouse> extractFromDataSet(DataTable table) 
        {
            List<WareHouse> list = new List<WareHouse>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    WareHouse wareHouse = new WareHouse();
                    wareHouse.WhsCode = row[0].ToString();
                    wareHouse.WhsName = row[1].ToString();
                    list.Add(wareHouse);
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