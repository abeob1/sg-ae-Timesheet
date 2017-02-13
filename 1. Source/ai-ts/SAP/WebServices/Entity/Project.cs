using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAP.WebServices
{
    public class Project
    {
        private string _PrjCode;
        private string _PrjName;
        private string _SrvType;
        private string _TimeEndDate;

        public string PrjCode
        {
            get { return _PrjCode; }
            set { _PrjCode = value; }
        }    

        public string PrjName
        {
            get { return _PrjName; }
            set { _PrjName = value; }
        }

        public string SrvType
        {
            get { return _SrvType; }
            set { _SrvType = value; }
        }

        public string TimeEndDate
        {
            get { return _TimeEndDate; }
            set { _TimeEndDate = value; }
        }

        public static List<Project> extractFromDataSet(DataTable table)
        {
            List<Project> list = new List<Project>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    Project proj = new Project();
                    proj.PrjCode = row[0].ToString();
                    proj.PrjName = row[1].ToString();
                    proj.SrvType = row["U_AI_SrvType"].ToString();
                    proj.TimeEndDate = row["U_AI_TimeEndDate"].ToString();
                    list.Add(proj);
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