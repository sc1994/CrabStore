using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 收货地址表  逻辑层
    /// </summary>
    public class CsAddressBll : BaseBll<CsAddress, CsAddressEnum, int>
    {
        public CsAddressBll() : base(new CsAddressDal()) { }

        public CsAddressBll(IBaseDal<CsAddress, CsAddressEnum, int> dal) : base(dal) { }
    }
}
