using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 订单表  逻辑层
    /// </summary>
    public class CsOrderBll : BaseBll<CsOrder, CsOrderEnum, string>
    {
        public CsOrderBll() : base(new CsOrderDal()) { }

        public CsOrderBll(IBaseDal<CsOrder, CsOrderEnum, string> dal) : base(dal) { }
    }
}
