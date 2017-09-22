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
        public int OrderId { get; set; } = ToInt("");

        /// <summary>
        /// 订单编号 客户显示所用 当前时间生产
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 下单用户
        /// </summary>
        public int UserId { get; set; } = ToInt("");

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalMoney { get; set; } = ToDecimal("");

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; } = ToDecimal("");

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal ActualMoney { get; set; } = ToDecimal("");

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate { get; set; } = ToDateTime("");

        /// <summary>
        /// 订单状态 0 取消订单 1已下单未至支付 2支付成功 3 配货中 4 已发货
        /// </summary>
        public int OrderState { get; set; } = ToInt("1");

        /// <summary>
        /// 订单地址信息
        /// </summary>
        public string OrderAddress { get; set; } = string.Empty;

        /// <summary>
        /// 寄件信息
        /// </summary>
        public string SendAddress { get; set; } = string.Empty;

        /// <summary>
        /// 快递单号
        /// </summary>
        public string OrderDelivery { get; set; } = string.Empty;

        /// <summary>
        /// 寄托物数量
        /// </summary>
        public int CargoNumber { get; set; } = ToInt("1");

        /// <summary>
        /// 份数 默认值1
        /// </summary>
        public int OrderCopies { get; set; } = ToInt("1");

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; } = ToDecimal("");

        /// <summary>
        /// 计费重量
        /// </summary>
        public decimal BillWeight { get; set; } = ToDecimal("");

        /// <summary>
        /// 数据状态 0 删除 1 有效
        /// </summary>
        public int RowStatus { get; set; } = ToInt("1");

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteDate { get; set; } = ToDateTime("1900-1-1");

        /// <summary>
        /// 删除描述
        /// </summary>
        public string DeleteDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 预支付编号
        /// </summary>
        public string PrepaymentId { get; set; } = string.Empty;

        /// <summary>
        /// 运费
        /// </summary>
        public decimal ExpressMoney { get; set; } = ToDecimal("0");

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceMoney { get; set; } = ToDecimal("0");

        /// <summary>
        /// 是否开票 0 不开票 1开票
        /// </summary>
        public int IsInvoice { get; set; } = ToInt("0");

        /// <summary>
        /// 订单备注
        /// </summary>
        public string OrderRemarks { get; set; } = string.Empty;

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
        /// 订单地址信息
        /// </summary>
        OrderAddress,
        /// <summary>
        /// 寄件信息
        /// </summary>
        SendAddress,
        /// <summary>
        /// 快递单号
        /// </summary>
        OrderDelivery,
        /// <summary>
        /// 寄托物数量
        /// </summary>
        CargoNumber,
        /// <summary>
        /// 份数 默认值1
        /// </summary>
        OrderCopies,
        /// <summary>
        /// 总重量
        /// </summary>
        TotalWeight,
        /// <summary>
        /// 计费重量
        /// </summary>
        BillWeight,
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
        /// <summary>
        /// 预支付编号
        /// </summary>
        PrepaymentId,
        /// <summary>
        /// 运费
        /// </summary>
        ExpressMoney,
        /// <summary>
        /// 服务费
        /// </summary>
        ServiceMoney,
        /// <summary>
        /// 是否开票 0 不开票 1开票
        /// </summary>
        IsInvoice,
        /// <summary>
        /// 订单备注
        /// </summary>
        OrderRemarks,
    }
}
