using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.Common;
using System.ComponentModel;

using System.Data.SqlClient;

namespace SAP.Admin
{
    public class oExcel
    {
        System.Data.OleDb.OleDbConnection con;
	    // Dim ExcelConnectionStr As String

        public oExcel(string dbPath)
	    {
		    string ExcelConnectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbPath + ";Extended Properties=Excel 8.0;";
		    con = new System.Data.OleDb.OleDbConnection(ExcelConnectionStr);
	    }

	    public DataTable GetDataSQL(string sqlQuery)
	    {
		    try {
			    con.Open();
			    System.Data.OleDb.OleDbCommand ExcelCommand = new System.Data.OleDb.OleDbCommand(sqlQuery, con);
			    System.Data.OleDb.OleDbDataReader Reader = null;
			    Reader = ExcelCommand.ExecuteReader();
			    DataTable dt = new DataTable();
			    dt.Load(Reader);
			    con.Close();
			    return dt;
		    } catch (Exception ex) {
			    con.Close();
			    return null;
		    }
	    }
	    public DataTable GetSheets()
	    {
		    try {
			    con.Open();
			    DataTable dt = null;
			    dt = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
			    con.Close();
			    return dt;
		    } catch (Exception ex) {
			   // MessageBox.Show(ex.ToString());
			    return null;
		    }
	    }
    }
}