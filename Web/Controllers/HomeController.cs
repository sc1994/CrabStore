using System.Linq;
using System.Web.Mvc;
using BLL;
using Common;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CsMenusBll _csMenusBll = new CsMenusBll();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            string where;
            if (CurrentUser.SysUserType == SysUserType.普通用户.GetHashCode())
            {
                where = "AND MenuName <> '用户管理'";
            }
            else
            {
                where = " AND 1=1";
            }
            var list = _csMenusBll.GetModelList(where);

            var res = list
                .Where(x => x.MenuParId == 0)
                .OrderByDescending(x => x.MenuOrder)
                .Select(p => new
                {
                    id = "0-" + p.MenuId.ToString(),
                    title = p.MenuName,
                    icon = p.MenuIcon,
                    url = p.MenuUrl,
                    child = list
                            .Where(x => x.MenuParId == p.MenuId)
                            .OrderByDescending(x => x.MenuOrder)
                            .Select(x => new
                            {
                                id = x.MenuParId + "-" + p.MenuId.ToString(),
                                title = x.MenuName,
                                url = x.MenuUrl
                            })
                });

            return Json(res);
        }
    }
}