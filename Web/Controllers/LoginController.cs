using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;
using Model.DBModel;
using SqlHelper;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
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
                    // ignored
                }
                if (ticket != null)
                {
                    var user = ticket.UserData.JsonToObject<CsSystemUsers>();
                    if (user != null)
                    {
                        return Redirect("~/");
                    }
                }
            }
            return View();
        }

        public ActionResult Login(string account, string password, bool remember)
        {
            var sh = new SqlHelper<CsSystemUsers>();
            sh.AddWhere(CsSystemUsersEnum.SysUserName, account);
            sh.AddWhere(CsSystemUsersEnum.SysUserPassword, password);
            var user = sh.Select().FirstOrDefault();
            if (user != null)
            {
                FormsAuthenticationTicket ticket;
                if (remember)
                {
                    ticket = new FormsAuthenticationTicket(1, account, DateTime.Now, DateTime.Now.AddDays(30), false, user.ToJson());
                }
                else
                {
                    ticket = new FormsAuthenticationTicket(1, account, DateTime.Now, DateTime.Now.AddHours(0.5), false, user.ToJson());
                }
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                Response.Cookies.Add(cookie);
                return Json(new { code = 1, data = "登陆成功" });
            }
            return Json(new { code = 0, data = "用户名或者密码错误" });
        }

        public ActionResult LogionOut()
        {
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie);
            }
            return Json(new { code = 1, data = "退出成功" });
        }
    }
}