using DAL;
using IDAL;
using Model.DBModel;
namespace BLL
{
    /// <summary>
    /// 后台管理菜单表  逻辑层
    /// </summary>
    public class CsMenusBll : BaseBll<CsMenus, CsMenusEnum, int>
    {
        public CsMenusBll() : base(new CsMenusDal()) { }

        public CsMenusBll(IBaseDal<CsMenus, CsMenusEnum, int> dal) : base(dal) { }
    }
}
