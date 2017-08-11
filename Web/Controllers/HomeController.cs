using System.Linq;
using System.Web.Mvc;
using BLL;

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
            var list = _csMenusBll.GetModelList("AND 1=1");

            var res = list
                .Where(x => x.MenuParId == 0)
                .OrderByDescending(x => x.MenuOrder)
                .Select(p => new
                {
                    id = p.MenuId.ToString(),
                    title = p.MenuName,
                    icon = p.MenuIcon,
                    url = p.MenuUrl,
                    child = list
                            .Where(x => x.MenuParId == p.MenuId)
                            .OrderByDescending(x => x.MenuOrder)
                            .Select(x => new
                            {
                                id = p.MenuId.ToString(),
                                title = x.MenuName,
                                url = x.MenuUrl
                            })
                });

            return Json(res);
        }
    }
}