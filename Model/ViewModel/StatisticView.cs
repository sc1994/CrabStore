namespace Model.ViewModel
{
    public class StatisticView
    {
        public class StatisticWhere
        {
            public int ProductType { get; set; } = 0;
            public int PartType { get; set; } = 0;
            public string ProductName { get; set; } = string.Empty;
        }

        public class StatisticList
        {
            public int OrderState { get; set; }
            public int? ProductId { get; set; }
            public string ProductType { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public decimal ProductWeight { get; set; }
            public decimal ProductPrice { get; set; }
            public string OrderId { get; set; } = string.Empty;
            public string OrderDate { get; set; } = string.Empty;
            public int ProductNumber { get; set; }
            public string ChoseType { get; set; } = string.Empty;
            public string Total { get; set; } = string.Empty;
            public string Stock { get; set; } = string.Empty;
            public string UnStork { get; set; } = string.Empty;
            public int UnStorkInt { get; set; }
        }
    }
}
