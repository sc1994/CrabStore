using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 商品表[螃蟹种类]  逻辑层
    /// </summary>
    public class CsProductsBll : BaseBll<CsProducts, CsProductsEnum, int>
    {
        public CsProductsBll() : base(new CsProductsDal()) { }

        public CsProductsBll(IBaseDal<CsProducts, CsProductsEnum, int> dal) : base(dal) { }
    }
}
