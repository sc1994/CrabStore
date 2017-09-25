
using System;
using System.Collections.Generic;
using Model.DBModel;

namespace Model.ViewModel
{
    public class CsOrderView
    {
        public class CsOrderWhere
        {
            public int RowStatus { get; set; } = -1;
            public List<string> Time { get; set; } = new List<string>();
            public int CurrentPage { get; set; } = 1;
            public decimal TotalStart { get; set; } = 0;
            public decimal TotalEnd { get; set; } = 0;
            public decimal DiscountStart { get; set; } = 0;
            public decimal DiscountEnd { get; set; } = 0;
            public decimal ActualStart { get; set; } = 0;
            public decimal ActualEnd { get; set; } = 0;
            public int Status { get; set; } = -1;
            public string UserName { get; set; } = string.Empty;
            public string UserPhone { get; set; } = string.Empty;
            public string OrderId { get; set; } = string.Empty;
            /// <summary>
            /// 订单源
            /// </summary>
            public string OrderSource { get; set; } = string.Empty;
            public string OrderNumber { get; set; } = string.Empty;
            public string IsInvoice { get; set; } = string.Empty;
            public string OrderAddress { get; set; } = string.Empty;
        }

        public class CsOrderPage : CsOrder
        {
            public string UserName { get; set; } = string.Empty;

            public string UserPhone { get; set; } = string.Empty;

            public string UserSex { get; set; } = string.Empty;
        }

        public class CsOrderInfo
        {
            public string TotalMoney { get; set; } = string.Empty;
            public string ActualMoney { get; set; } = string.Empty;
            public string DeleteDate { get; set; } = string.Empty;
            public string DeleteDescribe { get; set; } = string.Empty;
            public string DiscountMoney { get; set; } = string.Empty;
            public string OrderDate { get; set; } = string.Empty;
            public int OrderId { get; set; }
            public string OrderNumber { get; set; } = string.Empty;
            public string OrderState { get; set; } = string.Empty;
            public string RowStatus { get; set; } = string.Empty;
            public string OrderStateDescribe { get; set; } = string.Empty;
            public string RowStatusDescribe { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string UserPhone { get; set; } = string.Empty;
            /// <summary>
            /// 收件地址
            /// </summary>
            public string OrderAddress { get; set; } = string.Empty;
            /// <summary>
            /// 收件联系人
            /// </summary>
            public string OrderConsignee { get; set; } = string.Empty;
            /// <summary>
            /// 收件人联系方式     
            /// </summary>
            public string OrderTelPhone { get; set; } = string.Empty;
            /// <summary>
            /// 收件人详细地址
            /// </summary>
            public string OrderDetails { get; set; } = string.Empty;
            public string OrderDelivery { get; set; } = string.Empty;
            public string OrderCopies { get; set; } = string.Empty;
            public string TotalWeight { get; set; } = string.Empty;
            public string BillWeight { get; set; } = string.Empty;
            public string SendAddress { get; set; } = string.Empty;
            /// <summary>
            /// 发件联系人
            /// </summary>
            public string SendConsignee { get; set; } = string.Empty;
            /// <summary>
            /// 发件人联系方式
            /// </summary>
            public string SendTelPhone { get; set; } = string.Empty;
            public List<CsOrderDetailExtend> CsOrderDetails { get; set; } = new List<CsOrderDetailExtend>();
            /// <summary>
            /// 订单源
            /// </summary>
            public string OrderSource { get; set; } = string.Empty;
            /// <summary>
            /// 预支付编号
            /// </summary>
            public string PrepaymentId { get; set; } = string.Empty;
            public string IsInvoice { get; set; } = string.Empty;
            public string OrderRemarks { get; set; } = string.Empty;
        }

        public class CsOrderExcel
        {
            public string 用户订单号 { get; set; } = string.Empty;
            public string 寄件公司 { get; set; } = string.Empty;
            public string 寄联系人 { get; set; } = string.Empty;
            public string 寄联系电话 { get; set; } = string.Empty;
            public string 寄件地址 { get; set; } = string.Empty;
            public string 收件公司 { get; set; } = string.Empty;
            public string 联系人 { get; set; } = string.Empty;
            public string 联系电话 { get; set; } = string.Empty;
            public string 手机号码 { get; set; } = string.Empty;
            public string 收件详细地址 { get; set; } = string.Empty;
            public string 付款方式 { get; set; } = string.Empty;
            public string 第三方付月结卡号 { get; set; } = string.Empty;
            public string 寄托物品 { get; set; } = string.Empty;
            public string 寄托物内容 { get; set; } = string.Empty;
            public string 寄托物编号 { get; set; } = string.Empty;
            public string 寄托物数量 { get; set; } = string.Empty;
            public string 件数 { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 实际重量单位KG { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 计费重量单位KG { get; set; } = string.Empty;
            public string 业务类型 { get; set; } = string.Empty;
            public string 是否代收货款 { get; set; } = string.Empty;
            public string 代收货款金额 { get; set; } = string.Empty;
            public string 代收卡号 { get; set; } = string.Empty;
            public string 是否保价 { get; set; } = string.Empty;
            public string 保价金额 { get; set; } = string.Empty;
            public string 标准化包装单位元 { get; set; } = string.Empty;
            public string 其他费用单位元 { get; set; } = string.Empty;
            public string 个性化包装单位元 { get; set; } = string.Empty;
            public string 是否自取 { get; set; } = string.Empty;
            public string 是否签回单 { get; set; } = string.Empty;
            public string 是否定时派送 { get; set; } = string.Empty;
            public string 派送日期 { get; set; } = string.Empty;
            public string 派送时段 { get; set; } = string.Empty;
            public string 是否电子验收 { get; set; } = string.Empty;
            public string 拍照类型 { get; set; } = string.Empty;
            public string 是否保单配送 { get; set; } = string.Empty;
            public string 是否拍照验证 { get; set; } = string.Empty;
            public string 是否易碎件 { get; set; } = string.Empty;
            public string 易碎金额 { get; set; } = string.Empty;
            public string 是否票据专送 { get; set; } = string.Empty;
            public string 是否超长超重服务 { get; set; } = string.Empty;
            public string 超长超重服务费 { get; set; } = string.Empty;
            public string 是否上门安装 { get; set; } = string.Empty;
            public string 安装类型 { get; set; } = string.Empty;
            public string 收件员 { get; set; } = string.Empty;
            public string 寄件方签名 { get; set; } = string.Empty;
            public string 寄件日期 { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 签收短信通知MSG { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 派件出仓通知SMS { get; set; } = string.Empty;
            public string 寄方客户备注 { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 长单位CM { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 宽单位CM { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string 高单位CM { get; set; } = string.Empty;
            public string 扩展字段1 { get; set; } = string.Empty;
            public string 扩展字段2 { get; set; } = string.Empty;
            public string 扩展字段3 { get; set; } = string.Empty;
            public string 扩展字段4 { get; set; } = string.Empty;


        }

        public class CsOrderImport
        {
            public string 发货人 { get; set; } = string.Empty;
            public string 发货人电话 { get; set; } = string.Empty;
            public string 收货人 { get; set; } = string.Empty;
            public string 收货人电话 { get; set; } = string.Empty;
            public string 收货地址 { get; set; } = string.Empty;
            public string 商品编码 { get; set; } = string.Empty;
            public string 种类 { get; set; } = string.Empty;
            public string 数量 { get; set; } = string.Empty;
            public string 单价 { get; set; } = string.Empty;
            public string 类型 { get; set; } = string.Empty;
            public string 总金额 { get; set; } = string.Empty;
            public string 实收金额 { get; set; } = string.Empty;
            public string 货运单号 { get; set; } = string.Empty;
            public string 标准化包装 { get; set; } = string.Empty;
        }

        public class CsOrderDetailExtend
        {
            /// <summary>
            /// 商品名称  如  大宗采购/公2.5(1233456)
            /// </summary>
            public string ProductName { get; set; } = string.Empty;
            public string ChoseType { get; set; } = string.Empty;
            public int DetailId { get; set; }
            public int ProductId { get; set; }
            public int ProductNumber { get; set; }
            public string TotalPrice { get; set; } = string.Empty;
            public string UnitPrice { get; set; } = string.Empty;
        }

        /// <summary>
        /// 订单以及详细
        /// </summary>
        public class CsOrderAndDetail
        {
            public CsOrder CsOrder { get; set; } = new CsOrder();
            public List<CsOrderDetail> CsOrderDetails { get; set; } = new List<CsOrderDetail>();
        }

        public class CsOrderTotalByProduct
        {
            public int ProductId { get; set; }

            public int Total { get; set; }
        }

        public class CsOrderBatchDis
        {
            public string 订单编号 { get; set; } = string.Empty;
            public string 运单号 { get; set; } = string.Empty;
        }

        public class CsOrderUpdate
        {
            // ReSharper disable once InconsistentNaming
            public int id { get; set; } = 0;
            // ReSharper disable once InconsistentNaming
            public int rowStatus { get; set; } = 0;
            // ReSharper disable once InconsistentNaming
            public DateTime deleteDate { get; set; } = DateTime.Now;
            // ReSharper disable once InconsistentNaming
            public string deleteDescribe { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public int orderState { get; set; } = 0;
            // ReSharper disable once InconsistentNaming
            public string delivery { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string sendConsignee { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string sendTelphone { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string orderConsignee { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string orderTelphone { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public string orderDetails { get; set; } = string.Empty;
            // ReSharper disable once InconsistentNaming
            public int isInvoice { get; set; } = 0;
            // ReSharper disable once InconsistentNaming
            public string orderRemarks { get; set; } = string.Empty;
        }
    }
}
