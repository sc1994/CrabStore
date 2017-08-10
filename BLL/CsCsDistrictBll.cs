using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    ///   逻辑层
    /// </summary>
    public class CsCsDistrictBll : BaseBll<CsCsDistrict, CsCsDistrictEnum, object>
    {
        public CsCsDistrictBll() : base(new CsCsDistrictDal()) { }

        public CsCsDistrictBll(IBaseDal<CsCsDistrict, CsCsDistrictEnum, object> dal) : base(dal) { }
    }
}
