using Dapper;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Model.DBModel;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 配件表  数据访问层
    /// </summary>
    public partial class CsPartsDal : ICsPartsDal
    {
        public bool Exists(int primaryKey)
        {
            var strSql = "SELECT COUNT(1) FROM CrabShop.dbo.[CsParts] WHERE 1 = @primaryKey";
            var parameters = new { primaryKey };
            return DbClient.Excute(strSql, parameters) > 0;
        }

        public bool ExistsByWhere(string where)
            => DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsParts] WHERE 1 = 1 {where};") > 0;

        public int Add(CsParts model)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO CrabShop.dbo.[CsParts] (");
            strSql.Append("PartType,PartName,PartWeight,PartPrice,OperationDate,PartState,PartNumber,PartImage");
            strSql.Append(") VALUES (");
            strSql.Append("@PartType,@PartName,@PartWeight,@PartPrice,@OperationDate,@PartState,@PartNumber,@PartImage);");
            strSql.Append("SELECT @@IDENTITY");
            return DbClient.ExecuteScalar<int>(strSql.ToString(), model);
        }

        public bool Update(CsParts model)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsParts] SET ");
            strSql.Append("PartType = @PartType,PartName = @PartName,PartWeight = @PartWeight,PartPrice = @PartPrice,OperationDate = @OperationDate,PartState = @PartState,PartNumber = @PartNumber,PartImage = @PartImage");
            strSql.Append(" WHERE PartId = @PartId");
            return DbClient.Excute(strSql.ToString(), model) > 0;
        }

        public bool Update(Dictionary<CsPartsEnum, object> updates, string where)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsParts] SET ");
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
            var strSql = "DELETE FROM CrabShop.dbo.[CsParts] WHERE PartId = @primaryKey";
            return DbClient.Excute(strSql, new { primaryKey }) > 0;
        }

        public int DeleteByWhere(string where)
            => DbClient.Excute($"DELETE FROM CrabShop.dbo.[CsParts] WHERE 1 = 1 {where}");

        public CsParts GetModel(int primaryKey)
        {
            var strSql = "SELECT * FROM CrabShop.dbo.[CsParts] WHERE PartId = @primaryKey";
            return DbClient.Query<CsParts>(strSql, new { primaryKey }).FirstOrDefault();
        }

        public List<CsParts> GetModelList(string where)
        {
            var strSql = $"SELECT * FROM CrabShop.dbo.[CsParts] WHERE 1 = 1 {where}";
            return DbClient.Query<CsParts>(strSql).ToList();
        }

        public List<CsParts> GetModelPage(CsPartsEnum order, string where, int pageIndex, int pageSize, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT * FROM ( SELECT TOP ({pageSize})");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY {order} DESC ) AS ROWNUMBER,* ");
            strSql.Append(" FROM  CrabShop.dbo.[CsParts] ");
            strSql.Append($" WHERE 1 = 1 {where} ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsParts] WHERE 1 = 1 {where};");
            return DbClient.Query<CsParts>(strSql.ToString()).ToList();
        }

    }
}
