using Model.DBModel;
using System.Linq;

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
    }
}
