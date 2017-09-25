using DAL;
using IDAL;
using Model.DBModel;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// 收货地址表  逻辑层
    /// </summary>
    public class CsAddressBll : BaseBll<CsAddress, CsAddressEnum, int>
    {
        private readonly CsAddressDal addressDal = new CsAddressDal();
        public CsAddressBll() : base(new CsAddressDal()) { }

        public CsAddressBll(IBaseDal<CsAddress, CsAddressEnum, int> dal) : base(dal) { }

        public bool UpdateState(int addressId)
        {
            Dictionary<CsAddressEnum, object> updates = new Dictionary<CsAddressEnum, object>();
            updates.Add(CsAddressEnum.AddressState, 0);
            return addressDal.Update(updates, " and AddressId =" + addressId);
            
        }

        /// <summary>
        /// 修改用户默认收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ChooseAddress(CsAddress address)
        {
            return addressDal.ChooseAddress(address);
        }
    }
}
