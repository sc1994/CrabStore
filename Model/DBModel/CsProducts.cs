using System;

namespace Model.DBModel
{
    /// <summary>
    /// 商品表(螃蟹种类)
    /// </summary>
    public class CsProducts : BaseModel
    {
        public static string PrimaryKey = "ProductId";
        public static string IdentityKey = "ProductId";

        /// <summary>
        /// 螃蟹商品编号
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 螃蟹商品类型 1 大宗采购 2 包塘直补
        /// </summary>
        public int ProductType { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品重量
        /// </summary>
        public decimal ProductWeight { get; set; }

        /// <summary>
        /// 商品状态 1表示正常 2表示下架
        /// </summary>
        public int ProductState { get; set; }

    }


    public enum CsProductsEnum
    {
        /// <summary>
        /// 螃蟹商品编号
        /// </summary>
        ProductId,
        /// <summary>
        /// 螃蟹商品类型 1 大宗采购 2 包塘直补
        /// </summary>
        ProductType,
        /// <summary>
        /// 商品名称
        /// </summary>
        ProductName,
        /// <summary>
        /// 商品重量
        /// </summary>
        ProductWeight,
        /// <summary>
        /// 商品状态 1表示正常 2表示下架
        /// </summary>
        ProductState,
    }
}
