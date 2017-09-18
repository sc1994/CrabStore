using System.Linq;
using System.Web.Mvc;
using Common;
using Model.ViewModel;
using SqlHelper;

namespace Web.Controllers
{
    public class CsUsersController : BaseController
    {
        // GET: CsUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsUsersPage(CsUsersView.CsUsersWhere para, CsUsersView.CsUsersOrder order)
        {
            var orderSql = string.Empty;
            //if (order.Balance != -1)
            //    orderSql += $" UserBalance {(OrderEnum)order.Balance},";
            //if (order.Rebate != -1)
            //    orderSql += $" Rebate {(OrderEnum)order.Rebate},";
            if (order.TotalPrice != -1)
                orderSql += $" TotalPrice {(OrderEnum)order.TotalPrice},";

            var sh = new SqlHelper<CsUsersView.CsUsersPage>("V_UsersInfo")
            {
                PageConfig = new PageConfig
                {
                    PageSize = PageSize,
                    PageSortSql = orderSql.Trim(',').IsNullOrEmpty() ? " UserId DESC " : orderSql.Trim(','),
                    PageIndex = para.CurrentPage
                }
            };

            if (para.UserId > 0)
                sh.AddWhere("UserId", para.UserId);
            if (!para.UserName.IsNullOrEmpty())
                sh.AddWhere("UserName", para.UserName);
            if (!para.UserPhone.IsNullOrEmpty())
                sh.AddWhere("UserPhone", para.UserPhone);

            return Json(new
            {
                data = sh.Select().Select(x => new CsUsersView.CsUsersPage
                {
                    UserName = x.UserName,
                    UserPhone = x.UserPhone,
                    TotalPrice = "￥ " + x.TotalPrice.ToDecimal().ToString("N2"),
                    UserId = x.UserId,
                    Rebate = "￥ " + x.Rebate.ToDecimal().ToString("N2"),
                    UserBalance = "￥ " + x.UserBalance.ToDecimal().ToString("N2"),
                    TotalWeight = x.TotalWeight.ToDecimal().ToString("0.000") + " KG"
                }),
                sql = sh.SqlString.ToString(),
                total = sh.Total
            });
        }
    }
}