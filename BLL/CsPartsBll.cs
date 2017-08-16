using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 配件表  逻辑层
    /// </summary>
    public class CsPartsBll : BaseBll<CsParts, CsPartsEnum, int>
    {
        public CsPartsBll() : base(new CsPartsDal()) { }

        public CsPartsBll(IBaseDal<CsParts, CsPartsEnum, int> dal) : base(dal) { }
    }
}
