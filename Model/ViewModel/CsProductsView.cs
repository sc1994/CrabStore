namespace Model.ViewModel
{
    public class CsProductsView
    {
        public class CsProductsWhere
        {
            public string ProductName { get; set; } = string.Empty;
            public int ProductType { get; set; } = 0;
            public int ProductState { get; set; } = -1;
        }

        public class  CsProductsList
        {
            public int ProductId { get; set; } 
            public string ProductType { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public string ProductImage { get; set; } = string.Empty;
            public string ProductWeight { get; set; } = string.Empty;
            public string ProductPrice { get; set; } = string.Empty;
            public string ProductState { get; set; } = string.Empty;
            public string OperationDate { get; set; } = string.Empty;
        }


    }
}
