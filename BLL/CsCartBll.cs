using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 购物车表  逻辑层
    /// </summary>
    public class CsCartBll : BaseBll<CsCart, CsCartEnum, int>
    {
        public CsCartBll() : base(new CsCartDal()) { }

        public CsCartBll(IBaseDal<CsCart, CsCartEnum, int> dal) : base(dal) { }
    }
}
