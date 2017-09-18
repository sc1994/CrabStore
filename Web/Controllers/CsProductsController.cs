using System.Linq;
using System.Web.Mvc;
using BLL;
using Common;
using Model.ViewModel;

namespace Web.Controllers
{
    public class CsProductsController : BaseController
    {
        private readonly CsProductsBll _csProductsBll = new CsProductsBll();
        // GET: CsProducts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsProductsList()
        {
            var list = _csProductsBll.GetModelList(" AND 1=1");
            return Json(list.Select(x => new CsProductsView.CsProductsList
            {
                OperationDate = x.OperationDate.ToString("yyyy-M-d"),
                ProductId = x.ProductId,
                ProductState = ((ProductState)x.ProductState).ToString(),
                ProductName = $"{x.ProductName}({x.ProductNumber})",
                ProductType = ((ProductType)x.ProductType).ToString(),
                ProductImage = x.ProductImage,
                ProductPrice = x.ProductPrice.ToString("N2").ToDecimal(),
                ProductStock = x.ProductStock,
                ProductWeight = x.ProductWeight.ToString("0.000")
            }));
        }

        public ActionResult ChangeCsProductsStatus(int id)
        {
            if (id < 1)
            {
                return Json(new ResModel
                {
                    Data = "错误的Id",
                    ResStatus = ResStatue.No
                });
            }
            var model = _csProductsBll.GetModel(id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    Data = "Id未能查询到相应的数据,请刷新页面再试",
                    ResStatus = ResStatue.No
                });
            }
            var status = model.ProductState == ProductState.在售.GetHashCode() ? 0 : 1;
            model.ProductState = status;
            if (_csProductsBll.Update(model))
            {
                return Json(new ResModel
                {
                    Data = status == ProductState.在售.GetHashCode() ? "上架成功" : "下架成功",
                    ResStatus = ResStatue.Yes
                });
            }
            return Json(new ResModel
            {
                Data = "系统错误,请稍后再试",
                ResStatus = ResStatue.No
            });
        }

    }
}