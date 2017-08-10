using System;

namespace Model.DBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CsCsDistrict : BaseModel
    {
        public static string PrimaryKey = "";
        public static string IdentityKey = "";

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public int parent_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }

    }


    public enum CsCsDistrictEnum
    {
        /// <summary>
        /// 
        /// </summary>
        id,
        /// <summary>
        /// 
        /// </summary>
        name,
        /// <summary>
        /// 
        /// </summary>
        parent_id,
        /// <summary>
        /// 
        /// </summary>
        code,
        /// <summary>
        /// 
        /// </summary>
        sort,
    }
}
