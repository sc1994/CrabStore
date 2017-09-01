using System;
using System.Collections.Generic;
using Common;
using Model.ViewModel;

namespace DAL
{
    /// <summary>
    /// 订单表  数据访问扩展层(此类中的代码不会被覆盖)
    /// </summary>
    public partial class CsOrderDal
    {
        /// <summary>
        /// 根据产品编号查询销售总数
        /// </summary>
        /// <param name="productIds">产品编号</param>
        /// <returns></returns>
        public IEnumerable<CsOrderView.CsOrderTotalByProduct> TotalNumber(string productIds)
        {
            var strSql = " select a.ProductId,sum(ProductNumber) as Total from CsOrderDetail a " +
                         " inner join " +
                         " CsOrder b on a.OrderId = b.OrderId " +
                         " where a.ProductId IN(" + (productIds.IsNullOrEmpty() ? "0" : productIds) + ")" +
                         " and Convert(varchar(7),b.OrderDate,23)='" + DateTime.Now.ToString("yyyy-MM") + "' GROUP BY a.ProductId";
            return DbClient.Query<CsOrderView.CsOrderTotalByProduct>(strSql);
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
            var strSql = "select sum(ProductNumber) from CsOrderDetail a inner join CsOrder b on a.OrderId = b.OrderId where a.ProductId= " + productId
                        + " and Convert(varchar(7),b.OrderDate,23)='" + nowTime.ToString("yyyy-MM") + "'";
            return DbClient.ExecuteScalar<int>(strSql);
        }
    }
}
