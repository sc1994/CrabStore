
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
            public int OrderId { get; set; } = 0;
        }
    }
}
