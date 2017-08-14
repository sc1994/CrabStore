using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    #region ���ݿ������
    /// <summary>
    /// ���ݿ������
    /// </summary>
    public class DbClient
    {
        /// <summary>
        /// ִ�д��β�ѯ
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
        /// ִ����Ӱ������ �� sql
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
        /// ִ�д������Ĵ洢����
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
        /// ִ�д������Ĵ洢����(��ѯ)
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
        /// ִ�д������Ĵ洢����
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
    }

    /// <summary>
    /// ����Դ
    /// </summary>
    public class DataSource
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["DBString"].ConnectionString;
        /// <summary>
        /// ���ӳ�
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
