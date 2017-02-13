using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class BOM
    {
        private string itemCode;

        public string ItemCode
        {
            get { return itemCode; }
            set { itemCode = value; }
        }

        private string itemName;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public static List<BOM> extractFromDataSet(DataTable table) 
        {
            List<BOM> list = new List<BOM>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    BOM item = new BOM();
                    item.ItemCode = row[0].ToString();
                    item.ItemName = row[1].ToString();
                    list.Add(item);
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