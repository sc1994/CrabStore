
using System.Web.Mvc;
using Common;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected internal JsonResult Json(ResModel data)
        {
            var res = new JsonResult
            {
                Data = new
                {
                    code = data.ResStatus.GetHashCode(),
                    data = data.Data
                }
            };
            return res;
        }

        protected const int PageSize = 15;
    }

    public class ResModel
    {
        public ResStatue ResStatus { get; set; } = ResStatue.Yes;
        public object Data { get; set; }
    }
}