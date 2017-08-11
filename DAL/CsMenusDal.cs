using Dapper;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using Model.DBModel;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 后台管理菜单表  数据访问层
    /// </summary>
    public partial class CsMenusDal : ICsMenusDal
    {
        public bool Exists(int primaryKey)
        {
            var strSql = "SELECT COUNT(1) FROM CrabShop.dbo.[CsMenus] WHERE 1 = @primaryKey";
            var parameters = new { primaryKey };
            return DbClient.Excute(strSql, parameters) > 0;
        }

        public bool ExistsByWhere(string where)
            => DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsMenus] WHERE 1 = 1 {where};") > 0;

        public int Add(CsMenus model)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO CrabShop.dbo.[CsMenus] (");
            strSql.Append("MenuName,MenuUrl,MenuIcon,MenuParId,MenuState,Remarks,MenuOrder");
            strSql.Append(") VALUES (");
            strSql.Append("@MenuName,@MenuUrl,@MenuIcon,@MenuParId,@MenuState,@Remarks,@MenuOrder);");
            strSql.Append("SELECT @@IDENTITY");
            return DbClient.ExecuteScalar<int>(strSql.ToString(), model);
        }

        public bool Update(CsMenus model)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsMenus] SET ");
            strSql.Append("MenuName = @MenuName,MenuUrl = @MenuUrl,MenuIcon = @MenuIcon,MenuParId = @MenuParId,MenuState = @MenuState,Remarks = @Remarks,MenuOrder = @MenuOrder");
            strSql.Append(" WHERE MenuId = @MenuId");
            return DbClient.Excute(strSql.ToString(), model) > 0;
        }

        public bool Update(Dictionary<CsMenusEnum, object> updates, string where)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE CrabShop.dbo.[CsMenus] SET ");
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
            var strSql = "DELETE FROM CrabShop.dbo.[CsMenus] WHERE MenuId = @primaryKey";
            return DbClient.Excute(strSql, new { primaryKey }) > 0;
        }

        public int DeleteByWhere(string where)
            => DbClient.Excute($"DELETE FROM CrabShop.dbo.[CsMenus] WHERE 1 = 1 {where}");

        public CsMenus GetModel(int primaryKey)
        {
            var strSql = "SELECT * FROM CrabShop.dbo.[CsMenus] WHERE MenuId = @primaryKey";
            return DbClient.Query<CsMenus>(strSql, new { primaryKey }).FirstOrDefault();
        }

        public List<CsMenus> GetModelList(string where)
        {
            var strSql = $"SELECT * FROM CrabShop.dbo.[CsMenus] WHERE 1 = 1 {where}";
            return DbClient.Query<CsMenus>(strSql).ToList();
        }

        public List<CsMenus> GetModelPage(CsMenusEnum order, string where, int pageIndex, int pageSize, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT * FROM ( SELECT TOP ({pageSize})");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY {order} DESC ) AS ROWNUMBER,* ");
            strSql.Append(" FROM  CrabShop.dbo.[CsMenus] ");
            strSql.Append($" WHERE 1 = 1 {where} ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CrabShop.dbo.[CsMenus] WHERE 1 = 1 {where};");
            return DbClient.Query<CsMenus>(strSql.ToString()).ToList();
        }

    }
}
