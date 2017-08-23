using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 寄件方信息  逻辑层
    /// </summary>
    public class CsSendBll : BaseBll<CsSend, CsSendEnum, int>
    {
        public CsSendBll() : base(new CsSendDal()) { }

        public CsSendBll(IBaseDal<CsSend, CsSendEnum, int> dal) : base(dal) { }
    }
}
