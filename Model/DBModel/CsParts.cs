using System;

namespace Model.DBModel
{
    /// <summary>
    /// 配件表
    /// </summary>
    public class CsParts : BaseModel
    {
        public static string PrimaryKey { get; set; } = "PartId";
        public static string IdentityKey { get; set; } = "PartId";

        /// <summary>
        /// 配件编号
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// 配件类型 1必选配件 2可选配件
        /// </summary>
        public int PartType { get; set; }

        /// <summary>
        /// 配件名称
        /// </summary>
        public string PartName { get; set; } = string.Empty;

        /// <summary>
        /// 配件重量
        /// </summary>
        public decimal PartWeight { get; set; }

        /// <summary>
        /// 配件价格
        /// </summary>
        public decimal PartPrice { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperationDate { get; set; } = ToDateTime("");

        /// <summary>
        /// 配件状态 1可用  0 删除
        /// </summary>
        public int PartState { get; set; }

        /// <summary>
        /// 配件编码
        /// </summary>
        public string PartNumber { get; set; } = string.Empty;

        /// <summary>
        /// 配件图片
        /// </summary>
        public string PartImage { get; set; } = string.Empty;

    }


    public enum CsPartsEnum
    {
        /// <summary>
        /// 配件编号
        /// </summary>
        PartId,
        /// <summary>
        /// 配件类型 1必选配件 2可选配件
        /// </summary>
        PartType,
        /// <summary>
        /// 配件名称
        /// </summary>
        PartName,
        /// <summary>
        /// 配件重量
        /// </summary>
        PartWeight,
        /// <summary>
        /// 配件价格
        /// </summary>
        PartPrice,
        /// <summary>
        /// 操作日期
        /// </summary>
        OperationDate,
        /// <summary>
        /// 配件状态 1可用  0 删除
        /// </summary>
        PartState,
        /// <summary>
        /// 配件编码
        /// </summary>
        PartNumber,
        /// <summary>
        /// 配件图片
        /// </summary>
        PartImage,
    }
}
