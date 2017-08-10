using Dapper;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Model.DBModel;
using System.Text;

namespace DAL
{
    /// <summary>
    ///   数据访问层
    /// </summary>
    public partial class CsCsDistrictDal : ICsCsDistrictDal
    {
        public bool Exists(object primaryKey)
        {
            return false;
        }

        public bool ExistsByWhere(string where)
            => DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsCsDistrict] WHERE 1 = 1 {where};") > 0;

        public object Add(CsCsDistrict model)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO CrabShop.dbo.[CsCsDistrict] (");
            strSql.Append("id,name,parent_id,code,sort");
            strSql.Append(") VALUES (");
            strSql.Append("@id,@name,@parent_id,@code,@sort);");
            return DbClient.Excute(strSql.ToString(), model);
        }

        public bool Update(CsCsDistrict model)
        {
            return false;
        }

        public bool Update(Dictionary<CsCsDistrictEnum, object> updates, string where)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsCsDistrict] SET ");
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

        public bool Delete(object primaryKey)
        {
            return false;
        }

        public int DeleteByWhere(string where)
            => DbClient.Excute($"DELETE FROM CrabShop.dbo.[CsCsDistrict] WHERE 1 = 1 {where}");

        public CsCsDistrict GetModel(object primaryKey)
        {
            return null;
        }

        public List<CsCsDistrict> GetModelList(string where)
        {
            var strSql = $"SELECT * FROM CrabShop.dbo.[CsCsDistrict] WHERE 1 = 1 {where}";
            return DbClient.Query<CsCsDistrict>(strSql).ToList();
        }

        public List<CsCsDistrict> GetModelPage(CsCsDistrictEnum order, string where, int pageIndex, int pageSize, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT * FROM ( SELECT TOP ({pageSize})");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY {order} DESC ) AS ROWNUMBER,* ");
            strSql.Append(" FROM  CrabShop.dbo.[CsCsDistrict] ");
            strSql.Append($" WHERE 1 = 1 {where} ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsCsDistrict] WHERE 1 = 1 {where};");
            return DbClient.Query<CsCsDistrict>(strSql.ToString()).ToList();
        }

    }
}
