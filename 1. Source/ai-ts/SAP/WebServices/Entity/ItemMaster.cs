using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class ItemMaster
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

        public static List<ItemMaster> extractFromDataSet(DataTable table) 
        {
            List<ItemMaster> list = new List<ItemMaster>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    ItemMaster item = new ItemMaster();
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