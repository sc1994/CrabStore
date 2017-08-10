using System;

namespace Model.DBModel
{
    /// <summary>
    /// 全国省市区三级数据
    /// </summary>
    public class CsDistrict : BaseModel
    {
        public static string PrimaryKey = "id";
        public static string IdentityKey = "id";

        /// <summary>
        /// 编号 主键 自动增长
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// 父级编号
        /// </summary>
        public int parent_id { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string code { get; set; } = string.Empty;

        /// <summary>
        /// 排序编号
        /// </summary>
        public int sort { get; set; }

    }


    public enum CsDistrictEnum
    {
        /// <summary>
        /// 编号 主键 自动增长
        /// </summary>
        id,
        /// <summary>
        /// 名称
        /// </summary>
        name,
        /// <summary>
        /// 父级编号
        /// </summary>
        parent_id,
        /// <summary>
        /// 区域编码
        /// </summary>
        code,
        /// <summary>
        /// 排序编号
        /// </summary>
        sort,
    }
}
