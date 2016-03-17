using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace OracleConnect
{
  public class EntityTest
  {
    public static void EntityClient_EntitySQL()
    {
      string _cust = "a";
      using (EntityConnection _conn = new EntityConnection("Name=bitter"))
      {
        _conn.Open();
        EntityCommand _cmd = _conn.CreateCommand();
        _cmd.CommandText = @"SELECT name FROM Entities.Customers AS c WHERE c.name = @name";
        _cmd.Parameters.AddWithValue("name", _cust);
        DbDataReader _reader = _cmd.ExecuteReader(CommandBehavior.SequentialAccess);
        while (_reader.Read())
          Console.WriteLine(_reader["Address"].ToString());
        _reader.Close();

      }
    }
    //public static void ObjectService_EntitySQL()
    //{
    //  string _cust = "a";
    //  using (Entities entities = new Entities())
    //  {
    //    ObjectQuery<Customer> query = entities.CreateQuery<Customer>(
    //    "SELECT name FROM Entities.Customers AS c WHERE c.name = @name",
    //    new ObjectParameter("name", _cust)
    //    );
    //    foreach (Customer c in query)
    //      Console.WriteLine(c.Name);
    //  }
    //}

    public static void UseEF()
    {
      CustomerContext db;
      using (db = new CustomerContext())
      {
        //直接执行Sql语句
        db.Database.ExecuteSqlCommand("delete from Customer");

        var orders = new List<Customer>(){            
                    new Customer{ Id=1, Name="a", Address = "AAAAAAA"},
                    new Customer{ Id=2, Name="b", Address = "BBBBBBB"}
        };

        //提交到db
        db.SaveChanges();
        //查询
        var query = db.Customers.Where(c => c.Name == "a").AsQueryable();
        //输出Sql语句
        Console.WriteLine(query.ToString());
        List<Customer> custEntities = query.ToList();

        foreach (var cust in custEntities)
        {
          Console.WriteLine(String.Format("ID:{0}/Name:{1}/Address:{2}", cust.Id, cust.Name, cust.Address));
        }
      }

      Console.WriteLine("ok!");
      Console.Read();
    }
  }

  public static class SequenceHelper
  {
    public static int GetNextVal(this DbContext db, string sequenceName)
    {
      if (db.Database.Connection.State != ConnectionState.Open)
      {
        db.Database.Connection.Open();
      }
      var cmd = db.Database.Connection.CreateCommand();
      cmd.CommandText = String.Format("select {0}.nextval from dual", sequenceName);
      cmd.CommandType = CommandType.Text;

      int result = 0;
      int.TryParse(cmd.ExecuteScalar().ToString(), out result);

      return result;
    }
  }

  public class CustomerContext : DbContext
  {
    //private static string connString = "Provider=OraOLEDB.Oracle.1;User ID=system;Password=Zjl1987629;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = bitter)));";
    private static string connString = "Data Source=bitter;User ID=system;Password=Zjl1987629;";

    public CustomerContext() : base(connString) { }

    public DbSet<Customer> Customers { set; get; }


    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
  }
}
