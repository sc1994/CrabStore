using System;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.WeChatModel;

namespace Web.Controllers
{
    public class WeChatApiController : Controller
    {
        public ActionResult GetOpenId(string currentPage)
        {
            var url = string.Format(WeChatConfig.WeChatCodeUrl, WeChatConfig.AppId, HttpUtility.UrlEncode($"http://{Request.Url?.Authority}/WeChatApi/RedirectUri?currentPage={currentPage}"));
            ViewBag.CodeUrl = url;
            LogHelper.Log(ViewBag.CodeUrl, "url");
            return View();
        }

        public ActionResult RedirectUri(string code, string currentPage)
        {
            LogHelper.Log(code + "/" + currentPage, "code/currentPage");
            var opneId = HttpHelper.HttpGet(string.Format(WeChatConfig.OpenIdUrl, WeChatConfig.AppId, WeChatConfig.AppSecret, code));
            LogHelper.Log(opneId, "opneId");
            return Redirect($"{currentPage}{(currentPage.IndexOf("?", StringComparison.Ordinal) > -1 ? "&" : "?")}openId=");
        }

        public ActionResult GetAccessToken()
        {
            var data = HttpHelper.HttpGet(string.Format(WeChatConfig.AccessTokenUrl, WeChatConfig.AppId, WeChatConfig.AppSecret));
            return Content(data);
        }
    }
}