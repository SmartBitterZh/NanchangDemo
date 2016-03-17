using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper.PersistenceHelper
{
  public abstract class BaseADOPersistenceHelper : BasePersistenceHelper
  {
    private static DbConnection conn = null;
    public static DbConnection Connection { get { return conn; } protected set { conn = value; } }
    private DbCommand cmd = null;
    private static string connstr = "";
    public static string ConnectionString { get { return connstr; } protected set { connstr = value; } }

    public BaseADOPersistenceHelper(string connStr = "")
    { connstr = connStr; }

    protected abstract DbConnection CreateConnection();
    protected abstract DbDataAdapter CreateDataAdapter(string sqlStr);
    protected abstract DbCommand CreateCommand(string sqlStr = "");

    public void ConnectionDisport()
    {
      if (Connection != null && Connection.State != ConnectionState.Closed)
      {
        Connection.Close();
        Connection.Dispose();
      }

    }

    #region 建立数据库连接对象
    /// <summary>
    /// 建立数据库连接
    /// </summary>
    /// <returns>返回一个数据库的连接OleDbConnection对象</returns>
    private DbConnection init()
    {
      try
      {
        conn = CreateConnection();
        if (conn.State != ConnectionState.Open)
          conn.Open();
      }
      catch (Exception e)
      {
        throw new Exception(e.Message.ToString());
      }
      return conn;
    }
    #endregion

    #region 设置OleDbCommand对象
    /// <summary>
    /// 设置OleDbCommand对象       
    /// </summary>
    /// <param name="cmd">OleDbCommand对象 </param>
    /// <param name="cmdText">命令文本</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="cmdParms">参数集合</param>
    private void SetCommand(DbCommand cmd, string cmdText, CommandType cmdType, DbParameter[] cmdParms)
    {
      cmd.Connection = conn;
      cmd.CommandText = cmdText;
      cmd.CommandType = cmdType;
      if (cmdParms != null)
      {
        cmd.Parameters.AddRange(cmdParms);
      }
    }
    #endregion

    #region 执行相应的sql语句，返回相应的DataSet对象
    /// <summary>
    /// 执行相应的sql语句，返回相应的DataSet对象
    /// </summary>
    /// <param name="sqlstr">sql语句</param>
    /// <returns>返回相应的DataSet对象</returns>
    public DataSet GetDataSet(string sqlstr)
    {
      DataSet ds = new DataSet();
      try
      {
        init();
        DbDataAdapter ada = CreateDataAdapter(sqlstr);
        ada.Fill(ds);
        conn.Close();
      }
      catch (Exception e)
      {
        throw new Exception(e.Message.ToString());
      }
      return ds;
    }
    #endregion

    #region 执行相应的sql语句，返回相应的DataSet对象
    /// <summary>
    /// 执行相应的sql语句，返回相应的DataSet对象
    /// </summary>
    /// <param name="sqlstr">sql语句</param>
    /// <param name="tableName">表名</param>
    /// <returns>返回相应的DataSet对象</returns>
    public DataSet GetDataSet(string sqlstr, string tableName)
    {
      DataSet ds = new DataSet();
      try
      {
        init();
        DbDataAdapter ada = CreateDataAdapter(sqlstr);
        ada.Fill(ds, tableName);
        conn.Close();
      }
      catch (Exception e)
      {
        throw new Exception(e.Message.ToString());
      }
      return ds;
    }
    #endregion

    #region 执行不带参数sql语句，返回一个DataTable对象
    /// <summary>
    /// 执行不带参数sql语句，返回一个DataTable对象
    /// </summary>
    /// <param name="cmdText">相应的sql语句</param>
    /// <returns>返回一个DataTable对象</returns>
    public DataTable GetDataTable(string cmdText)
    {

      DbDataReader reader;
      DataTable dt = new DataTable();
      try
      {
        init();
        cmd = CreateCommand(cmdText);
        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        dt.Load(reader);
        reader.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return dt;
    }
    #endregion

    #region 执行带参数的sql语句或存储过程，返回一个DataTable对象
    /// <summary>
    /// 执行带参数的sql语句或存储过程，返回一个DataTable对象
    /// </summary>
    /// <param name="cmdText">sql语句或存储过程名</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="cmdParms">参数集合</param>
    /// <returns>返回一个DataTable对象</returns>
    public DataTable GetDataTable(string cmdText, CommandType cmdType, OleDbParameter[] cmdParms)
    {
      DbDataReader reader;
      DataTable dt = new DataTable();
      try
      {
        init();
        cmd = CreateCommand();
        SetCommand(cmd, cmdText, cmdType, cmdParms);
        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        dt.Load(reader);
        reader.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return dt;
    }
    #endregion

    #region 执行不带参数sql语句，返回所影响的行数
    /// <summary>
    /// 执行不带参数sql语句，返回所影响的行数
    /// </summary>
    /// <param name="cmdText">增，删，改sql语句</param>
    /// <returns>返回所影响的行数</returns>
    public int ExecuteNonQuery(string cmdText)
    {
      int count;
      try
      {
        init();
        cmd = CreateCommand(cmdText);
        count = cmd.ExecuteNonQuery();
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return count;
    }
    #endregion

    #region 执行带参数sql语句或存储过程，返回所影响的行数
    /// <summary>
    /// 执行带参数sql语句或存储过程，返回所影响的行数
    /// </summary>
    /// <param name="cmdText">带参数的sql语句和存储过程名</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="cmdParms">参数集合</param>
    /// <returns>返回所影响的行数</returns>
    public int ExecuteNonQuery(string cmdText, CommandType cmdType, OleDbParameter[] cmdParms)
    {
      int count;
      try
      {
        init();
        cmd = CreateCommand();
        SetCommand(cmd, cmdText, cmdType, cmdParms);
        count = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return count;
    }
    #endregion

    #region 执行不带参数sql语句，返回一个从数据源读取数据的OleDbDataReader对象
    /// <summary>
    /// 执行不带参数sql语句，返回一个从数据源读取数据的OleDbDataReader对象
    /// </summary>
    /// <param name="cmdText">相应的sql语句</param>
    /// <returns>返回一个从数据源读取数据的OleDbDataReader对象</returns>
    public DbDataReader ExecuteReader(string cmdText)
    {
      DbDataReader reader;
      try
      {
        init();
        cmd = CreateCommand(cmdText);
        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return reader;
    }
    #endregion

    #region 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的OleDbDataReader对象
    /// <summary>
    /// 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的OleDbDataReader对象
    /// </summary>
    /// <param name="cmdText">sql语句或存储过程名</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="cmdParms">参数集合</param>
    /// <returns>返回一个从数据源读取数据的OleDbDataReader对象</returns>
    public DbDataReader ExecuteReader(string cmdText, CommandType cmdType, OleDbParameter[] cmdParms)
    {
      DbDataReader reader;
      try
      {
        init();
        cmd = CreateCommand();
        SetCommand(cmd, cmdText, cmdType, cmdParms);
        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return reader;
    }
    #endregion

    #region 执行不带参数sql语句,返回结果集首行首列的值object
    /// <summary>
    /// 执行不带参数sql语句,返回结果集首行首列的值object
    /// </summary>
    /// <param name="cmdText">相应的sql语句</param>
    /// <returns>返回结果集首行首列的值object</returns>
    public object ExecuteScalar(string cmdText)
    {
      object obj;
      try
      {
        init();
        cmd = CreateCommand(cmdText);
        obj = cmd.ExecuteScalar();
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return obj;
    }
    #endregion

    #region 执行带参数sql语句或存储过程,返回结果集首行首列的值object
    /// <summary>
    /// 执行带参数sql语句或存储过程,返回结果集首行首列的值object
    /// </summary>
    /// <param name="cmdText">sql语句或存储过程名</param>
    /// <param name="cmdType">命令类型</param>
    /// <param name="cmdParms">返回结果集首行首列的值object</param>
    /// <returns></returns>
    public object ExecuteScalar(string cmdText, CommandType cmdType, OleDbParameter[] cmdParms)
    {
      object obj;
      try
      {
        init();
        cmd = CreateCommand();
        SetCommand(cmd, cmdText, cmdType, cmdParms);
        obj = cmd.ExecuteScalar();
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }
      return obj;
    }
    #endregion
  }
}
