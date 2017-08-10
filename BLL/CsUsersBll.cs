using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 用户表  逻辑层
    /// </summary>
    public class CsUsersBll : BaseBll<CsUsers, CsUsersEnum, int>
    {
        public CsUsersBll() : base(new CsUsersDal()) { }

        public CsUsersBll(IBaseDal<CsUsers, CsUsersEnum, int> dal) : base(dal) { }
    }
}
