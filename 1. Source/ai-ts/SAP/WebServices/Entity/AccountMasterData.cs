using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{    
    public class AccountMasterData
    {
        private string acctCode;

        public string AcctCode
        {
            get { return acctCode; }
            set { acctCode = value; }
        }

        private string acctName;

        public string AcctName
        {
            get { return acctName; }
            set { acctName = value; }
        }

        private string frgName;

        public string FrgName
        {
            get { return frgName; }
            set { frgName = value; }
        }

        public static List<AccountMasterData> extractFromDataSet(DataTable table) 
        {
            List<AccountMasterData> list = new List<AccountMasterData>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    AccountMasterData acc = new AccountMasterData();
                    acc.AcctCode = row[0].ToString();
                    acc.AcctName = row[1].ToString();
                    acc.FrgName = row[2].ToString();
                    list.Add(acc);
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