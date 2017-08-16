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
        public CsOrderBll() : base(new CsOrderDal()) { }

        public CsOrderBll(IBaseDal<CsOrder, CsOrderEnum, int> dal) : base(dal) { }

        public int TotalNumber(int productId, DateTime nowTime)
        {
            CsOrderDal order = new CsOrderDal();
            int total = order.TotalNumber(productId, nowTime);
            return total;
        }
    }
}
