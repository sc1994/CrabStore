using DAL;
using IDAL;
using Model.DBModel;
using System;
namespace BLL
{
    /// <summary>
    /// 订单表  逻辑层
    /// </summary>
    public class CsOrderBll : BaseBll<CsOrder, CsOrderEnum, int>
    {
        private readonly CsOrderDal _dal = new CsOrderDal();

        public CsOrderBll() : base(new CsOrderDal()) { }

        public CsOrderBll(IBaseDal<CsOrder, CsOrderEnum, int> dal) : base(dal) { }

        public int TotalNumber(int productId,DateTime nowTime)
        {
            return _dal.TotalNumber(productId, nowTime);
        }
    }
}
