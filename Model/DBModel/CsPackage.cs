using System;

namespace Model.DBModel
{
    /// <summary>
    /// 商品套餐
    /// </summary>
    public class CsPackage : BaseModel
    {
        public static string PrimaryKey { get; set; } = "PackageId";
        public static string IdentityKey { get; set; } = "PackageId";

        /// <summary>
        /// 套餐编号
        /// </summary>
        public int PackageId { get; set; } = ToInt("");

        /// <summary>
        /// 1 大宗采购 2蟹塘直捕
        /// </summary>
        public int PackageType { get; set; } = ToInt("1");

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// 套餐编码
        /// </summary>
        public string PackageNumber { get; set; } = string.Empty;

        /// <summary>
        /// 套餐图片
        /// </summary>
        public string PackageImage { get; set; } = string.Empty;

        /// <summary>
        /// 套餐重量
        /// </summary>
        public decimal PackageWeight { get; set; } = ToDecimal("0");

        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal PackagePrice { get; set; } = ToDecimal("0");

        /// <summary>
        /// 套餐状态 1表示正常 0 表示下架
        /// </summary>
        public int PackageState { get; set; } = ToInt("");

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDate { get; set; } = ToDateTime("getdate");

        /// <summary>
        /// 套餐库存
        /// </summary>
        public int PackageStock { get; set; } = ToInt("");

    }


    public enum CsPackageEnum
    {
        /// <summary>
        /// 套餐编号
        /// </summary>
        PackageId,
        /// <summary>
        /// 1 大宗采购 2蟹塘直捕
        /// </summary>
        PackageType,
        /// <summary>
        /// 套餐名称
        /// </summary>
        PackageName,
        /// <summary>
        /// 套餐编码
        /// </summary>
        PackageNumber,
        /// <summary>
        /// 套餐图片
        /// </summary>
        PackageImage,
        /// <summary>
        /// 套餐重量
        /// </summary>
        PackageWeight,
        /// <summary>
        /// 套餐价格
        /// </summary>
        PackagePrice,
        /// <summary>
        /// 套餐状态 1表示正常 0 表示下架
        /// </summary>
        PackageState,
        /// <summary>
        /// 操作时间
        /// </summary>
        OperationDate,
        /// <summary>
        /// 套餐库存
        /// </summary>
        PackageStock,
    }
}
