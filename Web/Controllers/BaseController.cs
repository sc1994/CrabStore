using System;
using Common;
using Model.DBModel;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Mvc.Filters;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected CsSystemUsers CurrentUser;

        protected string OrderNumberFormat = "yyyyMMddHHmmssffff";

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

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = null;
                try
                {
                    ticket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch (Exception)
                {
                    LogOut(filterContext);
                }
                if (ticket != null)
                {
                    CurrentUser = ticket.UserData.JsonToObject<CsSystemUsers>();
                    ViewBag.UserName = CurrentUser.SysUserName;
                    if (CurrentUser == null)
                    {
                        LogOut(filterContext);
                    }
                }
                else
                {
                    LogOut(filterContext);
                }
            }
            else
            {
                LogOut(filterContext);
            }
        }

        private void LogOut(AuthenticationContext filterContext)
        {
            filterContext.HttpContext.Response.Write("<script>window.location.href = '/Login'</script>");
            HttpContext.Response.End();
        }
    }

    public class ResModel
    {
        public ResStatue ResStatus { get; set; } = ResStatue.Yes;
        public object Data { get; set; }
    }
}