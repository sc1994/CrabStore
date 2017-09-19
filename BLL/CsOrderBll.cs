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
        private readonly CsOrderDetailBll _orderBll = new CsOrderDetailBll();

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
            return _dal.TotalNumber(productId, nowTime);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int AddOrder(OrderModel order,out string orderNumber)
        {
            return _dal.AddOrder(order,out orderNumber);
        }

        /// <summary>
        /// 订单添加预支付编号
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="prepaymentId">预支付编号</param>
        /// <returns></returns>
        public int UpdatePrepaymentId(int orderId, string prepaymentId)
        {
            return _dal.UpdatePrepaymentId(orderId, prepaymentId);
        }

        /// <summary>
        /// 订单修改状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        public int UpdateOrderState(int orderId, int orderState)
        {
            return _dal.UpdateOrderState(orderId, orderState);
        }

        /// <summary>
        /// 根据openId查询订单列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public List<CsOrder> GetModelListByOpenId(string openId, int num, int size, out int total)
        {
            return _dal.GetModelListByOpenId(openId, num, size, out total);
        }

        /// <summary>
        /// 支付完成，修改订单状态，生成扣除库存与修改用户购买重量
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int FinshOrder(int orderId, int userId, decimal totalWeight, int orderCopies)
        {
            return _dal.FinshOrder(orderId, userId, totalWeight, orderCopies);
        }

        #region 套餐订单处理
        /// <summary>
        /// 添加套餐订单
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="orderNumber">返回订单编号</param>
        /// <returns></returns>
        public int AddPackageOrder(OrderModel order,out string orderNumber)
        {
            return _dal.AddPackageOrder(order, out orderNumber);
        }

        /// <summary>
        /// 完成套餐订单支付操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int FinshPackageOrder(int orderId)
        {
            return _dal.FinshPackageOrder(orderId);
        }

        #endregion
    }
}