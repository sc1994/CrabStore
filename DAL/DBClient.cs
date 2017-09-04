using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    #region 数据库操作类
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DbClient
    {
        /// <summary>
        /// 执行带参查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = DataSource.GetConnection())
            {
                try
                {
                    var tList = con.Query<T>(sql, param);
                    con.Close();
                    return tList;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "------------ SQL:" + sql);
                }
            }
        }

        /// <summary>
        /// 执行受影响行数 的 sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Excute(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = DataSource.GetConnection())
            {
                int line;
                try
                {
                    line = con.Execute(sql, param, transaction);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "-------------- SQL:" + sql);
                }
                return line;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(string sql, object param = null)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = DataSource.GetConnection())
            {
                return con.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strProcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteScalarProc<T>(string strProcName, object param = null)
        {
            using (var con = DataSource.GetConnection())
            {
                return (T)con.ExecuteScalar(strProcName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程(查询)
        /// </summary>
        /// <param name="strProcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteQueryProc<T>(string strProcName, object param = null)
        {
            using (var con = DataSource.GetConnection())
            {
                var tList = con.Query<T>(strProcName, param, commandType: CommandType.StoredProcedure);
                con.Close();
                return tList;
            }
        }

        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="strProcName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteProc(string strProcName, object param = null)
        {
            try
            {
                using (var con = DataSource.GetConnection())
                {
                    return con.Execute(strProcName, param, commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, SqlTransaction trans,string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (SqlException e)
                {
                    //trans.Rollback();
                    //LogHelper.Log("sql:" + SQLString + "; msg:" + e.Message, "执行SQL异常");
                    throw new Exception(e.Message + "---------SQL:" + SQLString);
                }
            }
        }

        /// <summary>
        /// 2012-2-29新增重载，执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction对象</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (SqlException e)
                {
                    //trans.Rollback();
                    //LogHelper.Log("sql:" + SQLString + "; msg:" + e.Message, "执行SQL异常");
                    throw new Exception(e.Message + "---------SQL:" + SQLString);
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }

    /// <summary>
    /// 数据源
    /// </summary>
    public class DataSource
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["DBString"].ConnectionString;
        /// <summary>
        /// 连接池
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetConnection()
        {
            if (string.IsNullOrEmpty(ConnString))
                throw new NoNullAllowedException(nameof(ConnString));
            return new SqlConnection(ConnString);
        }
        
    }

    
    #endregion
}
