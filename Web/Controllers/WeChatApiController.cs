using System;
using System.Web.Mvc;
using Model.WeChatModel;

namespace Web.Controllers
{
    public class WeChatApiController : Controller
    {
        public ActionResult GetOpenId(string currentPage)
        {
            var url = string.Format(WeChatConfig.WeChatCodeUrl, WeChatConfig.AppId, $"http://{Request.Url?.Authority}/WeChatApi/RedirectUri?currentPage={currentPage}");
            return Content("");
        }

        public ActionResult RedirectUri(string code, string currentPage)
        {
            return Redirect($"{currentPage}{(currentPage.IndexOf("?", StringComparison.Ordinal) > -1 ? "&" : "?")}openId=");
        }
    }
}