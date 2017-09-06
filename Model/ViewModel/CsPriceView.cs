using System.Collections.Generic;

namespace Model.ViewModel
{
    public class CsPriceView
    {
        public class CsPriceWhere
        {
            public List<string> Time { get; set; } = new List<string>();
            public decimal PriceStart { get; set; }
            public decimal PriceEnd { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int CurrentPage { get; set; }
            public int ProductType { get; set; } = 0;
        }

        public class CsPricePage
        {
            public int PriceId { get; set; }
            public string ProductId { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public string CurrentPrice { get; set; } = string.Empty;
            /// <summary>
            /// 历史价格
            /// </summary>
            public string PriceNumber { get; set; } = string.Empty;
            public string PriceDate { get; set; } = string.Empty;
            public string ProductType { get; set; } = string.Empty;
            public string ProductNumber { get; set; } = string.Empty;
        }
    }
}
