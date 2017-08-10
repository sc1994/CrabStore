
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected internal JsonResult Json(ResModel data)
        {
            var res = new JsonResult();
            return res;
        }
    }

    public class ResModel
    {

    }
}