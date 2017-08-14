using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 系统用户表  逻辑层
    /// </summary>
    public class CsSystemUsersBll : BaseBll<CsSystemUsers, CsSystemUsersEnum, int>
    {
        public CsSystemUsersBll() : base(new CsSystemUsersDal()) { }

        public CsSystemUsersBll(IBaseDal<CsSystemUsers, CsSystemUsersEnum, int> dal) : base(dal) { }
    }
}
