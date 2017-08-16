using DAL;
using IDAL;
using Model.DBModel;
using System;

namespace BLL
{
    /// <summary>
    /// 商品价格[螃蟹价格]  逻辑层
    /// </summary>
    public class CsPriceBll : BaseBll<CsPrice, CsPriceEnum, int>
    {
        public CsPriceBll() : base(new CsPriceDal()) { }

        public CsPriceBll(IBaseDal<CsPrice, CsPriceEnum, int> dal) : base(dal) { }

        
    }
}
