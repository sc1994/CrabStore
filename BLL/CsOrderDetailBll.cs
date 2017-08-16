using DAL;
using IDAL;
using System;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 订单详细表  逻辑层
    /// </summary>
    public class CsOrderDetailBll : BaseBll<CsOrderDetail, CsOrderDetailEnum, int>
    {
        public CsOrderDetailBll() : base(new CsOrderDetailDal()) { }

        public CsOrderDetailBll(IBaseDal<CsOrderDetail, CsOrderDetailEnum, int> dal) : base(dal) { }

        
    }
}
