using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 全国省市区三级数据  逻辑层
    /// </summary>
    public class CsDistrictBll : BaseBll<CsDistrict, CsDistrictEnum, int>
    {
        public CsDistrictBll() : base(new CsDistrictDal()) { }

        public CsDistrictBll(IBaseDal<CsDistrict, CsDistrictEnum, int> dal) : base(dal) { }
    }
}
