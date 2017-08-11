using System;

namespace Model.DBModel
{
    /// <summary>
    /// 商品价格[螃蟹价格]
    /// </summary>
    public class CsPrice : BaseModel
    {
        public static string PrimaryKey { get; set; } = "PriceId";
        public static string IdentityKey { get; set; } = "PriceId";

        /// <summary>
        /// 商品价格编号
        /// </summary>
        public int PriceId { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal PriceNumber { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 价格时间
        /// </summary>
        public DateTime PriceDate { get; set; } = ToDateTime("");

    }


    public enum CsPriceEnum
    {
        /// <summary>
        /// 商品价格编号
        /// </summary>
        PriceId,
        /// <summary>
        /// 商品价格
        /// </summary>
        PriceNumber,
        /// <summary>
        /// 商品编号
        /// </summary>
        ProductId,
        /// <summary>
        /// 价格时间
        /// </summary>
        PriceDate,
    }
}
