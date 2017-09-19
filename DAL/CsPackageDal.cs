using Dapper;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Model.DBModel;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 商品套餐  数据访问层
    /// </summary>
    public partial class CsPackageDal : ICsPackageDal
    {
        public bool Exists(int primaryKey)
        {
            var strSql = "SELECT COUNT(1) FROM CrabShop.dbo.[CsPackage] WHERE 1 = @primaryKey";
            var parameters = new { primaryKey };
            return DbClient.Excute(strSql, parameters) > 0;
        }

        public bool ExistsByWhere(string where)
            => DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsPackage] WHERE 1 = 1 {where};") > 0;

        public int Add(CsPackage model)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO CrabShop.dbo.[CsPackage] (");
            strSql.Append("PackageType,PackageName,PackageNumber,PackageImage,PackageWeight,PackagePrice,PackageState,OperationDate,PackageStock");
            strSql.Append(") VALUES (");
            strSql.Append("@PackageType,@PackageName,@PackageNumber,@PackageImage,@PackageWeight,@PackagePrice,@PackageState,@OperationDate,@PackageStock);");
            strSql.Append("SELECT @@IDENTITY");
            return DbClient.ExecuteScalar<int>(strSql.ToString(), model);
        }

        public bool Update(CsPackage model)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsPackage] SET ");
            strSql.Append("PackageType = @PackageType,PackageName = @PackageName,PackageNumber = @PackageNumber,PackageImage = @PackageImage,PackageWeight = @PackageWeight,PackagePrice = @PackagePrice,PackageState = @PackageState,OperationDate = @OperationDate,PackageStock = @PackageStock");
            strSql.Append(" WHERE PackageId = @PackageId");
            return DbClient.Excute(strSql.ToString(), model) > 0;
        }

        public bool Update(Dictionary<CsPackageEnum, object> updates, string where)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsPackage] SET ");
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
            var strSql = "DELETE FROM CrabShop.dbo.[CsPackage] WHERE PackageId = @primaryKey";
            return DbClient.Excute(strSql, new { primaryKey }) > 0;
        }

        public int DeleteByWhere(string where)
            => DbClient.Excute($"DELETE FROM CrabShop.dbo.[CsPackage] WHERE 1 = 1 {where}");

        public CsPackage GetModel(int primaryKey)
        {
            var strSql = "SELECT * FROM CrabShop.dbo.[CsPackage] WHERE PackageId = @primaryKey";
            return DbClient.Query<CsPackage>(strSql, new { primaryKey }).FirstOrDefault();
        }

        public List<CsPackage> GetModelList(string where)
        {
            var strSql = $"SELECT * FROM CrabShop.dbo.[CsPackage] WHERE 1 = 1 {where}";
            return DbClient.Query<CsPackage>(strSql).ToList();
        }

        public List<CsPackage> GetModelPage(CsPackageEnum order, string where, int pageIndex, int pageSize, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT * FROM ( SELECT TOP ({pageSize})");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY {order} DESC ) AS ROWNUMBER,* ");
            strSql.Append(" FROM  CrabShop.dbo.[CsPackage] ");
            strSql.Append($" WHERE 1 = 1 {where} ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsPackage] WHERE 1 = 1 {where};");
            return DbClient.Query<CsPackage>(strSql.ToString()).ToList();
        }

    }
}
