using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{    
    public class Promotion
    {
        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _ProCode;

        public string ProCode
        {
            get { return _ProCode; }
            set { _ProCode = value; }
        }
        private string _ProName;

        public string ProName
        {
            get { return _ProName; }
            set { _ProName = value; }
        }
        private string _ProValue;

        public string ProValue
        {
            get { return _ProValue; }
            set { _ProValue = value; }
        }
        private string _ProQty;

        public string ProQty
        {
            get { return _ProQty; }
            set { _ProQty = value; }
        }
        private string _ProTrf;

        public string ProTrf
        {
            get { return _ProTrf; }
            set { _ProTrf = value; }
        }
        private string _ItemCode;

        public string ItemCode
        {
            get { return _ItemCode; }
            set { _ItemCode = value; }
        }
        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }
        private string _Condition;

        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private string _IsReplace;

        public string IsReplace
        {
            get { return _IsReplace; }
            set { _IsReplace = value; }
        }
        private string _Sole;

        public string Sole
        {
            get { return _Sole; }
            set { _Sole = value; }
        }
        private string _ChkCond;

        public string ChkCond
        {
            get { return _ChkCond; }
            set { _ChkCond = value; }
        }
        private string _HeadDscAmt;

        public string HeadDscAmt
        {
            get { return _HeadDscAmt; }
            set { _HeadDscAmt = value; }
        }
        private string _HeadDscPer;

        public string HeadDscPer
        {
            get { return _HeadDscPer; }
            set { _HeadDscPer = value; }
        }
        private string _WhsCode;

        public string WhsCode
        {
            get { return _WhsCode; }
            set { _WhsCode = value; }
        }


        public static List<Promotion> extractFromDataSet(DataTable table) 
        {
            List<Promotion> list = new List<Promotion>();
            try
            {                
                foreach (DataRow row in table.Rows)
                {
                    Promotion promo = new Promotion();
                    promo.ID = row[0].ToString();
                    promo.ProCode = row[1].ToString();
                    promo.ProName = row[2].ToString();
                    promo.ProValue = row[3].ToString();
                    promo.ProQty = row[4].ToString();
                    promo.ProTrf = row[5].ToString();
                    promo.ItemCode = row[6].ToString();
                    promo.ItemName = row[7].ToString();
                    promo.Condition = row[8].ToString();
                    promo.IsReplace = row[9].ToString();
                    promo.Sole = row[10].ToString();
                    promo.ChkCond = row[11].ToString();
                    promo.HeadDscAmt = row[12].ToString();
                    promo.HeadDscPer = row[13].ToString();
                    promo.WhsCode = row[14].ToString();
                    list.Add(promo);
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