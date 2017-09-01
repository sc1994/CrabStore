using DAL;
using IDAL;
using Model.DBModel;
using System;
using System.Collections.Generic;
using Model.ViewModel;

namespace BLL
{
    /// <summary>
    /// 订单表  逻辑层
    /// </summary>
    public class CsOrderBll : BaseBll<CsOrder, CsOrderEnum, int>
    {
        private readonly CsOrderDal _dal = new CsOrderDal();

        public CsOrderBll() : base(new CsOrderDal())
        {
        }

        public CsOrderBll(IBaseDal<CsOrder, CsOrderEnum, int> dal) : base(dal)
        {
        }

        /// <summary>
        /// 根据产品编号查询销售总数
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public IEnumerable<CsOrderView.CsOrderTotalByProduct> TotalNumber(string productIds)
        {
            return _dal.TotalNumber(productIds);
        }
        /// <summary>
        /// 根据产品编号与月份查询销售总数
        /// 根据产品编号查询销售总数
        /// </summary>
        /// <param name="productId">产品编号</param>
        /// <param name="nowTime">月份</param>
        /// <returns></returns>
        public int TotalNumber(int productId, DateTime nowTime)
        {
            return _dal.TotalNumber(productId,nowTime);
        }
    }
}
