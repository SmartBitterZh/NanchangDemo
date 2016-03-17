using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;

namespace OracleConnect
{
  class Program
  {
    static void Main(string[] args)
    {
      //Console.WriteLine(OraOLEDBConnect());
      //EntityTest.UseEF();
      



      Console.ReadKey();
    }

    public static string OracleClientConnect()
    {
      string _connectionStr = "Data Source=bitter;User ID=system;Password=Zjl1987629;";
      OracleConnection _conn = null;
      try
      {
        _conn = new OracleConnection(_connectionStr);
        _conn.Open();
        return "Connect Successfull";
      }
      catch (Exception e)
      {
        return e.Message;
      }
      finally
      {
        if (_conn != null && _conn.State == System.Data.ConnectionState.Connecting)
          _conn.Dispose();
      }
    }
    public static string OraOLEDBConnect()
    {
      //string connString = "Provider=OraOLEDB.Oracle.1;User ID=datacenter2013;Password=asdefg;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 180.166.92.235)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = MYSTEEL)));";
      string connString = "Provider=OraOLEDB.Oracle.1;User ID=system;Password=Zjl1987629;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = bitter)));";
      OleDbConnection conn = new OleDbConnection(connString);
      try
      {
        conn.Open();
        OleDbCommand _comm;
        object _result;
        string _sql = "create table Customer(Id int,Name varchar2(50),Address varchar2(100))";
        using (_comm = new OleDbCommand("select Name from Customer", conn))
        {
          _result = _comm.ExecuteScalar();
        }
        return _result.ToString();
      }
      catch (Exception ex)
      {
        return ex.Message.ToString();
      }
      finally
      {
        conn.Close();
      }
    }
  }


  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
  }


  public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
  }

}


