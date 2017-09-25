using System;

namespace Model.DBModel
{
    /// <summary>
    /// 收货地址表
    /// </summary>
    public class CsAddress : BaseModel
    {
        public static string PrimaryKey { get; set; } = "AddressId";
        public static string IdentityKey { get; set; } = "AddressId";

        /// <summary>
        /// 收货地址编号 主键 自动增长
        /// </summary>
        public int AddressId { get; set; } = ToInt("");

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; } = ToInt("");

        /// <summary>
        /// 收件公司名称
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; } = string.Empty;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; } = string.Empty;

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelPhone { get; set; } = string.Empty;

        /// <summary>
        /// 性别 先生/女士
        /// </summary>
        public string ConSex { get; set; } = string.Empty;

        /// <summary>
        /// 是否默认地址 1默认 2不默认
        /// </summary>
        public int IsDefault { get; set; } = ToInt("");

        /// <summary>
        /// 地址状态 0 已删除 1正常
        /// </summary>
        public int AddressState { get; set; } = ToInt("1");

    }


    public enum CsAddressEnum
    {
        /// <summary>
        /// 收货地址编号 主键 自动增长
        /// </summary>
        AddressId,
        /// <summary>
        /// 用户编号
        /// </summary>
        UserId,
        /// <summary>
        /// 收件公司名称
        /// </summary>
        CompanyName,
        /// <summary>
        /// 收货人
        /// </summary>
        Consignee,
        /// <summary>
        /// 联系电话
        /// </summary>
        Mobile,
        /// <summary>
        /// 详细地址
        /// </summary>
        Details,
        /// <summary>
        /// 手机号码
        /// </summary>
        TelPhone,
        /// <summary>
        /// 性别 先生/女士
        /// </summary>
        ConSex,
        /// <summary>
        /// 是否默认地址 1默认 2不默认
        /// </summary>
        IsDefault,
        /// <summary>
        /// 地址状态 0 已删除 1正常
        /// </summary>
        AddressState,
    }
}
