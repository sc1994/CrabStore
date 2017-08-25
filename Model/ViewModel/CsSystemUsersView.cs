namespace Model.ViewModel
{
    public class CsSystemUsersView
    {
        public class CsSystemUsersWhere
        {
            public string SysUserName { get; set; } = string.Empty;
            public int SysUserState { get; set; } = -1;
            public int SysUserType { get; set; } = 0;
            public int CurrentPage { get; set; } = 1;
        }

        public class CsSystemUsersPage
        {
            public int SysUserId { get; set; }
            public string SysUserName { get; set; } = string.Empty;
            public string SysUserPassword { get; set; } = string.Empty;
            public string SysUserType { get; set; } = string.Empty;
            public string SysUserState { get; set; } = string.Empty;
            public string SysUserDate { get; set; } = string.Empty;
            public string DeleteDate { get; set; } = string.Empty;
            public string DeleteDescribe { get; set; } = string.Empty;
        }
    }
}
