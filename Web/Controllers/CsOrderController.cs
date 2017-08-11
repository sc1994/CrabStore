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
        private readonly CsUsersBll _csUsersBll = new CsUsersBll();

        // GET: CsOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsOrderPage(CsOrderView.CsOrderWhere para)
        {
            var sh = new SqlHelper<CsOrder>
            {
                PageConfig = new PageConfig
                {
                    PageIndex = para.CurrentPage,
                    PageSize = PageSize,
                    PageSortField = CsOrderEnum.OrderId.ToString(),
                    SortEnum = SortEnum.Desc
                }
            };
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
            if (!para.UserName.IsNullOrEmpty())
            {
                var userList = _csUsersBll.GetModelList($" AND UserName LIKE '%{para.UserName}%'");
                if (userList.Any())
                {
                    sh.AddWhere(CsOrderEnum.UserId, string.Join(",", userList.Select(x => x.UserId)), RelationEnum.In);
                }
            }

            return Json(new
            {
                data = sh.Select(),
                sql = sh.SqlString.ToString()
            });
        }
    }
}