namespace Model.ViewModel
{
    public class CsUsersView
    {
        public class CsUsersWhere
        {
            public int UserId { get; set; } = 0;
            public string UserName { get; set; } = string.Empty;
            public string UserPhone { get; set; } = string.Empty;
            public int CurrentPage { get; set; } = 0;
        }

        public class CsUsersOrder
        {
            public int Balance { get; set; } = -1;
            public int Rebate { get; set; } = -1;
            public int TotalPrice { get; set; } = -1;
        }

        public class CsUsersPage
        {
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string UserPhone { get; set; } = string.Empty;
            public string Rebate { get; set; } = string.Empty;
            public string TotalPrice { get; set; } = string.Empty;
            public string UserBalance { get; set; } = string.Empty;
            public  string TotalWeight { get; set; } = string.Empty;
        }
    }
}
