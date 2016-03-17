using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBControl
{
  public class DBControl
  {
    private static OleDbConnection conn;
    public static OleDbConnection Connection
    {
      get
      {
        if (conn == null)
        {
          string connString = "Provider=OraOLEDB.Oracle.1;User ID=system;Password=Zjl1987629;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = bitter)));";
          conn = new OleDbConnection(connString);
          try
          {
            conn.Open();
            //string _sql = "Create table Product(ID int,Name varchar2(50), RecommendedRetailPrice decimal, SellingPrice decimal)";
            //string _sql = "insert into Product(1,'a',10,15)";
            //string _sql = "insert into Product(2,'b',20,25)";
            //using (OleDbCommand _cmd = new OleDbCommand(_sql, Connection))
            //{
            //  _cmd.ExecuteNonQuery();
            //}
          }
          finally
          {
            conn.Close();
          }
        }
        return conn;
      }
    }


    public static DataTable FindAll()
    {
      try
      {
        OleDbDataAdapter _adp;
        DataTable _table = new DataTable();
        string _sql = "select * from Product";
        using (_adp = new OleDbDataAdapter(_sql, Connection))
        {
          _adp.Fill(_table);
        }
        return _table;
      }
      finally
      {
        conn.Close();
      }
    }
  }
}
