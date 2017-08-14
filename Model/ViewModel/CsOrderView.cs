
using System.Collections.Generic;
using Model.DBModel;

namespace Model.ViewModel
{
    public class CsOrderView
    {
        public class CsOrderWhere
        {
            public int RowStatus { get; set; } = -1;
            public string Time { get; set; } = string.Empty;
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
            public int OrderId { get; set; } = 0;
        }

        public class CsOrderPage : CsOrder
        {
            public string UserName { get; set; } = string.Empty;

            public string UserPhone { get; set; } = string.Empty;

            public string UserSex { get; set; } = string.Empty;
        }

        public class CsOrderInfo
        {
            public  string TotalMoney { get; set; } = string.Empty;
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
            public List<CsOrderDetailExtend> CsOrderDetails { get; set; } = new List<CsOrderDetailExtend>();
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
    }
}
