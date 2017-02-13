using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{    
    public class BusinessPartner
    {
        private string cardCode;

        public string CardCode
        {
            get { return cardCode; }
            set { cardCode = value; }
        }

        private string cardName;

        public string CardName
        {
            get { return cardName; }
            set { cardName = value; }
        }
               
        public static List<BusinessPartner> extractFromDataSet(DataTable table ) 
        {
            List<BusinessPartner> list = new List<BusinessPartner>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    BusinessPartner partner = new BusinessPartner();
                    partner.CardCode = row[0].ToString();
                    partner.CardName = row[1].ToString();
                    list.Add(partner);
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