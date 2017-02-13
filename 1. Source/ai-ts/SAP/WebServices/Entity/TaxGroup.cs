using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class TaxGroup
    {
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string _Rate;

        public string Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }

        public static List<TaxGroup> extractFromDataSet(DataTable table)
        {
            List<TaxGroup> list = new List<TaxGroup>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    TaxGroup tax = new TaxGroup();
                    tax.Code = row[0].ToString();
                    tax.Name = row[1].ToString();
                    if ("".Equals(row[2].ToString()))
                        tax.Rate = row[1].ToString();
                    else
                        tax.Rate = row[2].ToString();
                    list.Add(tax);
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