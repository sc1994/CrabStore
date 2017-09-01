using Model.DBModel;
using System.Linq;

namespace DAL
{
    /// <summary>
    /// 全国省市区三级数据  数据访问扩展层(此类中的代码不会被覆盖)
    /// </summary>
    public partial class CsDistrictDal
    {
        public CsDistrict GetModel(string strWhere)
        {
            var strSql = "SELECT top 1 * FROM CrabShop.dbo.[CsDistrict] WHERE "+strWhere;
            return DbClient.Query<CsDistrict>(strSql, null).FirstOrDefault();;
        }
    }
}
