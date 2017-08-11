using System;
using System.Linq;
using System.Web.Mvc;
using BLL;
using Common;
using Model.DBModel;
using Model.ViewModel;
using SqlHelper;

namespace Web.Controllers
{
    public class CsOrderController : BaseController
    {
        // GET: CsOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsOrderPage(CsOrderView.CsOrderWhere para)
        {
            var sh = new SqlHelper<CsOrderView.CsOrderPage>("CsOrder")
            {
                PageConfig = new PageConfig
                {
                    PageIndex = para.CurrentPage,
                    PageSize = PageSize,
                    PageSortField = CsOrderEnum.OrderId.ToString(),
                    SortEnum = SortEnum.Desc
                },
                Alia = "co"
            };
            sh.AddShow(CsOrderEnum.OrderId);
            sh.AddShow(CsOrderEnum.OrderNumber);
            sh.AddShow("co." + CsOrderEnum.UserId);
            sh.AddShow(CsOrderEnum.TotalMoney);
            sh.AddShow(CsOrderEnum.DiscountMoney);
            sh.AddShow(CsOrderEnum.ActualMoney);
            sh.AddShow(CsOrderEnum.OrderDate);
            sh.AddShow(CsOrderEnum.OrderState);
            sh.AddShow(CsOrderEnum.RowStatus);
            sh.AddShow(CsOrderEnum.DeleteDate);
            sh.AddShow(CsOrderEnum.DeleteDescribe);
            sh.AddShow(CsUsersEnum.UserName);
            sh.AddShow(CsUsersEnum.UserPhone);
            sh.AddShow(CsUsersEnum.UserSex);

            sh.AddJoin(JoinEnum.LeftJoin, "CsUsers", "cu", "UserId", "UserId");

            if (para.RowStatus > -1)
                sh.AddWhere(CsOrderEnum.RowStatus, para.RowStatus);
            if (para.ActualStart > 0)
                sh.AddWhere(CsOrderEnum.ActualMoney, para.ActualStart, RelationEnum.GreaterEqual);
            if (para.ActualEnd > 0)
                sh.AddWhere(CsOrderEnum.ActualMoney, para.ActualEnd, RelationEnum.LessEqual);
            if (para.DiscountStart > 0)
                sh.AddWhere(CsOrderEnum.DiscountMoney, para.DiscountStart, RelationEnum.GreaterEqual);
            if (para.DiscountEnd > 0)
                sh.AddWhere(CsOrderEnum.DiscountMoney, para.DiscountEnd, RelationEnum.LessEqual);
            if (para.TotalStart > 0)
                sh.AddWhere(CsOrderEnum.TotalMoney, para.TotalStart, RelationEnum.GreaterEqual);
            if (para.TotalEnd > 0)
                sh.AddWhere(CsOrderEnum.TotalMoney, para.TotalEnd, RelationEnum.LessEqual);
            if (para.OrderId > 0)
                sh.AddWhere(CsOrderEnum.OrderId, para.OrderId);
            if (para.Status > -1)
                sh.AddWhere(CsOrderEnum.OrderState, para.Status);
            if (!para.UserName.IsNullOrEmpty())
                sh.AddWhere("cu." + CsUsersEnum.UserName, para.UserName, RelationEnum.Like);
            if (!para.UserPhone.IsNullOrEmpty())
                sh.AddWhere("cu." + CsUsersEnum.UserPhone, para.UserPhone, RelationEnum.Like);
            if (para.Time != null)
            {
                var timeArr = para.Time.Split(new[] { "," }, StringSplitOptions.None);
                if (!timeArr[0].IsNullOrEmpty())
                {
                    sh.AddWhere(CsOrderEnum.OrderDate, timeArr[0], RelationEnum.GreaterEqual);
                }
                if (!timeArr[1].IsNullOrEmpty())
                {
                    sh.AddWhere(CsOrderEnum.OrderDate, timeArr[1], RelationEnum.LessEqual);
                }
            }

            var list = sh.Select().Select(x => new
            {
                x.OrderId,
                x.OrderNumber,
                x.UserId,
                TotalMoney = x.TotalMoney.ToString("C2"),
                DiscountMoney = x.DiscountMoney.ToString("C2"),
                ActualMoney = x.ActualMoney.ToString("C2"),
                OrderDate = x.OrderDate.ToString("yyyy-M-d hh:mm:ss"),
                OrderState = ((OrderState)x.OrderState).ToString(),
                RowStatus = ((RowStatus)x.RowStatus).ToString(),
                DeleteDate = x.DeleteDate.ToString("yyyy-M-d hh:mm:ss"),
                x.DeleteDescribe,
                UserName = x.UserName + $"({x.UserSex})",
                x.UserPhone,
            });

            return Json(new
            {
                data = list,
                sql = sh.SqlString.ToString(),
                total = sh.Total
            });
        }
    }
}