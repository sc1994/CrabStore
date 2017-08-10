using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 返利表  逻辑层
    /// </summary>
    public class CsRebateBll : BaseBll<CsRebate, CsRebateEnum, int>
    {
        public CsRebateBll() : base(new CsRebateDal()) { }

        public CsRebateBll(IBaseDal<CsRebate, CsRebateEnum, int> dal) : base(dal) { }
    }
}
