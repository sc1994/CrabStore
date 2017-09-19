using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 商品套餐  逻辑层
    /// </summary>
    public class CsPackageBll : BaseBll<CsPackage, CsPackageEnum, int>
    {
        public CsPackageBll() : base(new CsPackageDal()) { }

        public CsPackageBll(IBaseDal<CsPackage, CsPackageEnum, int> dal) : base(dal) { }
    }
}
