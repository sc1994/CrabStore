using Model.DBModel;
using Model.ViewModel;
using System.Data;
using System.Linq;
using System.Text;
namespace DAL
{
    /// <summary>
    /// 用户表  数据访问扩展层(此类中的代码不会被覆盖)
    /// </summary>
    public partial class CsUsersDal
    {
        public CsUsers GetModel(string openId)
        {
            var strSql = "SELECT * FROM CrabShop.dbo.[CsUsers] WHERE OpenId = @openId";
            return DbClient.Query<CsUsers>(strSql, new { openId }).FirstOrDefault();
        }
        public CsUsers GetModelByTelPhone(string telPhone)
        {
            var strSql = $"select top 1 * from CsUsers where UserPhone='{telPhone}' ";
            return DbClient.Query<CsUsers>(strSql).FirstOrDefault();
        }

        /// <summary>
        /// 根据openId查询该用户的返利信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public UserRebateView GetUserRebateInfo(string openId)
        {
            var strSql = new StringBuilder();
            strSql.Append("select a.UserId,a.UserName,a.UserBalance,a.TotalWight,SUM(b.RebateMoney) as RebateMoney,");
            strSql.Append("SUM(c.DiscountMoney) as DiscountMoney from CsUsers a left join CsRebate b on a.UserId =b.UserId");
            strSql.Append("left join CsOrder c on a.UserId =c.UserId");
            strSql.Append($"where a.OpenId='{openId}'");
            strSql.Append("group by a.UserId,a.UserName,a.UserBalance,a.TotalWight");
            return DbClient.Query<UserRebateView>(strSql.ToString()).FirstOrDefault();            
        }
    }
}
