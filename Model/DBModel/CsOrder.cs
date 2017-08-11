using System;

namespace Model.DBModel
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class CsOrder : BaseModel
    {
        public static string PrimaryKey { get; set; } = "OrderId";
        public static string IdentityKey { get; set; } = "OrderId";

        /// <summary>
        /// 订单序号，主键、自动增长
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 订单编号 客户显示所用 当前时间生产
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 下单用户
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal ActualMoney { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate { get; set; } = ToDateTime("");

        /// <summary>
        /// 订单状态 0 取消订单 1已下单未至支付 2支付成功 3 配货中 4 已发货
        /// </summary>
        public int OrderState { get; set; }

        /// <summary>
        /// 数据状态 0 删除 1 有效
        /// </summary>
        public int RowStatus { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteDate { get; set; } = ToDateTime("1900-1-1");

        /// <summary>
        /// 删除描述
        /// </summary>
        public string DeleteDescribe { get; set; } = string.Empty;

    }


    public enum CsOrderEnum
    {
        /// <summary>
        /// 订单序号，主键、自动增长
        /// </summary>
        OrderId,
        /// <summary>
        /// 订单编号 客户显示所用 当前时间生产
        /// </summary>
        OrderNumber,
        /// <summary>
        /// 下单用户
        /// </summary>
        UserId,
        /// <summary>
        /// 总金额
        /// </summary>
        TotalMoney,
        /// <summary>
        /// 优惠金额
        /// </summary>
        DiscountMoney,
        /// <summary>
        /// 实际金额
        /// </summary>
        ActualMoney,
        /// <summary>
        /// 下单时间
        /// </summary>
        OrderDate,
        /// <summary>
        /// 订单状态 0 取消订单 1已下单未至支付 2支付成功 3 配货中 4 已发货
        /// </summary>
        OrderState,
        /// <summary>
        /// 数据状态 0 删除 1 有效
        /// </summary>
        RowStatus,
        /// <summary>
        /// 删除时间
        /// </summary>
        DeleteDate,
        /// <summary>
        /// 删除描述
        /// </summary>
        DeleteDescribe,
    }
}
