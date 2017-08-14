using System;

namespace Model.DBModel
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    public class CsSystemUsers : BaseModel
    {
        public static string PrimaryKey { get; set; } = "SysUserId";
        public static string IdentityKey { get; set; } = "SysUserId";

        /// <summary>
        /// 系统用户编号
        /// </summary>
        public int SysUserId { get; set; }

        /// <summary>
        /// 系统用户名
        /// </summary>
        public string SysUserName { get; set; } = string.Empty;

        /// <summary>
        /// 系统用户密码
        /// </summary>
        public string SysUserPassword { get; set; } = string.Empty;

        /// <summary>
        /// 1 管理员 2普通用户
        /// </summary>
        public int SysUserType { get; set; }

        /// <summary>
        /// 1正常 2已删除
        /// </summary>
        public int SysUserState { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime SysUserDate { get; set; } = ToDateTime("");

    }


    public enum CsSystemUsersEnum
    {
        /// <summary>
        /// 系统用户编号
        /// </summary>
        SysUserId,
        /// <summary>
        /// 系统用户名
        /// </summary>
        SysUserName,
        /// <summary>
        /// 系统用户密码
        /// </summary>
        SysUserPassword,
        /// <summary>
        /// 1 管理员 2普通用户
        /// </summary>
        SysUserType,
        /// <summary>
        /// 1正常 2已删除
        /// </summary>
        SysUserState,
        /// <summary>
        /// 操作时间
        /// </summary>
        SysUserDate,
    }
}
