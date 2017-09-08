using DAL;
using IDAL;
using Model.DBModel;
using Model.ViewModel;

namespace BLL
{
    /// <summary>
    /// 用户表  逻辑层
    /// </summary>
    public class CsUsersBll : BaseBll<CsUsers, CsUsersEnum, int>
    {
        private readonly CsUsersDal userDAL = new CsUsersDal();
        public CsUsersBll() : base(new CsUsersDal()) { }

        public CsUsersBll(IBaseDal<CsUsers, CsUsersEnum, int> dal) : base(dal) { }

        public CsUsers GetModel(string openId)
        {
            return userDAL.GetModel(openId);
        }
        public CsUsers GetModelByTelPhone(string telPhone)
        {
            return userDAL.GetModelByTelPhone(telPhone);
        }
        public UserRebateView GetUserRebateInfo(string openId)
        {
            return userDAL.GetUserRebateInfo(openId);
        }
    }
}
