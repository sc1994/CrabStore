using Dapper;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Model.DBModel;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 收货地址表  数据访问层
    /// </summary>
    public partial class CsAddressDal : ICsAddressDal
    {
        public bool Exists(int primaryKey)
        {
            var strSql = "SELECT COUNT(1) FROM CrabShop.dbo.[CsAddress] WHERE 1 = @primaryKey";
            var parameters = new { primaryKey };
            return DbClient.Excute(strSql, parameters) > 0;
        }

        public bool ExistsByWhere(string where)
            => DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsAddress] WHERE 1 = 1 {where};") > 0;

        public int Add(CsAddress model)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO CrabShop.dbo.[CsAddress] (");
            strSql.Append("UserId,CompanyName,Consignee,Mobile,Details,TelPhone,ConSex,IsDefault,AddressState");
            strSql.Append(") VALUES (");
            strSql.Append("@UserId,@CompanyName,@Consignee,@Mobile,@Details,@TelPhone,@ConSex,@IsDefault,@AddressState);");
            strSql.Append("SELECT @@IDENTITY");
            return DbClient.ExecuteScalar<int>(strSql.ToString(), model);
        }

        public bool Update(CsAddress model)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsAddress] SET ");
            strSql.Append("UserId = @UserId,CompanyName = @CompanyName,Consignee = @Consignee,Mobile = @Mobile,Details = @Details,TelPhone = @TelPhone,ConSex = @ConSex,IsDefault = @IsDefault,AddressState = @AddressState");
            strSql.Append(" WHERE AddressId = @AddressId");
            return DbClient.Excute(strSql.ToString(), model) > 0;
        }

        public bool Update(Dictionary<CsAddressEnum, object> updates, string where)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsAddress] SET ");
            var para = new DynamicParameters();
            foreach (var update in updates)
            {
                strSql.Append($" {update.Key} = @{update.Key},");
                para.Add(update.Key.ToString(), update.Value);
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append($" WHERE 1=1 {where}");
            return DbClient.Excute(strSql.ToString(), para) > 0;
        }

        public bool Delete(int primaryKey)
        {
            var strSql = "DELETE FROM CrabShop.dbo.[CsAddress] WHERE AddressId = @primaryKey";
            return DbClient.Excute(strSql, new { primaryKey }) > 0;
        }

        public int DeleteByWhere(string where)
            => DbClient.Excute($"DELETE FROM CrabShop.dbo.[CsAddress] WHERE 1 = 1 {where}");

        public CsAddress GetModel(int primaryKey)
        {
            var strSql = "SELECT * FROM CrabShop.dbo.[CsAddress] WHERE AddressId = @primaryKey";
            return DbClient.Query<CsAddress>(strSql, new { primaryKey }).FirstOrDefault();
        }

        public List<CsAddress> GetModelList(string where)
        {
            var strSql = $"SELECT * FROM CrabShop.dbo.[CsAddress] WHERE 1 = 1 {where}";
            return DbClient.Query<CsAddress>(strSql).ToList();
        }

        public List<CsAddress> GetModelPage(CsAddressEnum order, string where, int pageIndex, int pageSize, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT * FROM ( SELECT TOP ({pageSize})");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY {order} DESC ) AS ROWNUMBER,* ");
            strSql.Append(" FROM  CrabShop.dbo.[CsAddress] ");
            strSql.Append($" WHERE 1 = 1 {where} ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsAddress] WHERE 1 = 1 {where};");
            return DbClient.Query<CsAddress>(strSql.ToString()).ToList();
        }

    }
}
