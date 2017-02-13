using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class SalesBuyerMasterData
    {
        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public static List<SalesBuyerMasterData> extractFromDataSet(DataTable table)
        {
            List<SalesBuyerMasterData> list = new List<SalesBuyerMasterData>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    SalesBuyerMasterData data = new SalesBuyerMasterData();
                    data.Code = row[0].ToString();
                    data.Name = row[1].ToString();
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