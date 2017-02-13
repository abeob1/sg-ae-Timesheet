using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class CostCenter
    {
        private string _PrcCode;

        public string PrcCode
        {
            get { return _PrcCode; }
            set { _PrcCode = value; }
        }

        private string _PrcName;

        public string PrcName
        {
            get { return _PrcName; }
            set { _PrcName = value; }
        }

        public static List<CostCenter> extractFromDataSet(DataTable table)
        {
            List<CostCenter> list = new List<CostCenter>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    CostCenter cc = new CostCenter();
                    cc.PrcCode = row[0].ToString();
                    cc.PrcName = row[1].ToString();
                    list.Add(cc);
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