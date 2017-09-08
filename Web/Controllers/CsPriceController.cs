
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using Common;
using Model.DBModel;
using Model.ViewModel;
using Model.WeChatModel;
using SqlHelper;

namespace Web.Controllers
{
    public class CsPriceController : BaseController
    {
        private readonly CsPriceBll _csPriceBll = new CsPriceBll();
        // GET: CsPrice
        public ActionResult Index()
        {
            ViewBag.Host = WeChatConfig.WeChatHost;
            ViewBag.wechat = WeChatConfig.WeChatHome;
            ViewBag.TempId = WeChatConfig.TemplatePrice;
            return View();
        }

        public ActionResult SubmitCsPrice(List<CsProducts> products)
        {
            var updateStatus = new List<bool>();
            foreach (var product in products)
            {
                var sh = new SqlHelper<CsProducts>();
                sh.AddUpdate(CsProductsEnum.ProductPrice.ToString(), product.ProductPrice);
                sh.AddUpdate(CsProductsEnum.ProductStock.ToString(), product.ProductStock);
                if (product.ProductStock <= 0)
                {
                    sh.AddUpdate(CsProductsEnum.ProductState.ToString(), ProductState.已下架.GetHashCode());
                }
                else
                {
                    sh.AddUpdate(CsProductsEnum.ProductState.ToString(), ProductState.在售.GetHashCode());
                }
                sh.AddUpdate(CsProductsEnum.OperationDate.ToString(), DateTime.Now);
                sh.AddWhere(CsProductsEnum.ProductId, product.ProductId);
                if (sh.Update() > 0)
                {
                    var status = _csPriceBll.Add(new CsPrice
                    {
                        ProductId = product.ProductId,
                        PriceDate = DateTime.Now,
                        PriceNumber = product.ProductPrice
                    }) > 0;
                    if (status)
                    {
                        updateStatus.Add(true);
                    }
                }
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = $"成功更新{updateStatus.Count(x => x)}条价格"
            });
        }

        public ActionResult GetCsPricePage(CsPriceView.CsPriceWhere para)
        {
            var sh = new SqlHelper<CsPriceView.CsPricePage>("CsPrice")
            {
                Alia = "price",
                PageConfig = new PageConfig
                {
                    PageSize = PageSize,
                    SortEnum = SortEnum.Desc,
                    PageSortField = CsPriceEnum.PriceId.ToString(),
                    PageIndex = para.CurrentPage
                }
            };
            sh.AddShow(CsPriceEnum.PriceNumber);
            sh.AddShow("price." + CsPriceEnum.ProductId);
            sh.AddShow(CsPriceEnum.PriceDate);
            sh.AddShow(CsPriceEnum.PriceId);
            sh.AddShow(CsProductsEnum.ProductName);
            sh.AddShow(CsProductsEnum.ProductType);
            sh.AddShow(CsProductsEnum.ProductNumber);
            sh.AddShow(CsProductsEnum.ProductPrice + " AS CurrentPrice");

            sh.AddJoin(JoinEnum.LeftJoin, "CsProducts", "product", "ProductId", "ProductId");

            if (!para.ProductName.IsNullOrEmpty())
                sh.AddWhere(CsProductsEnum.ProductName, para.ProductName, RelationEnum.Like);
            if (para.PriceStart > 0)
                sh.AddWhere(CsPriceEnum.PriceNumber, para.PriceStart, RelationEnum.GreaterEqual);
            if (para.PriceEnd > 0)
                sh.AddWhere(CsPriceEnum.PriceNumber, para.PriceEnd, RelationEnum.LessEqual);
            if (para.ProductType != 0)
                sh.AddWhere(CsProductsEnum.ProductType, para.ProductType);
            if (para.Time.Count > 0)
            {
                if (!para.Time[0].IsNullOrEmpty())
                    sh.AddWhere(CsPriceEnum.PriceDate, para.Time[0], RelationEnum.GreaterEqual);
                if (!para.Time[1].IsNullOrEmpty())
                    sh.AddWhere(CsPriceEnum.PriceDate, para.Time[1], RelationEnum.LessEqual);
            }

            var list = sh.Select();

            return Json(new
            {
                data = list.Select(x => new CsPriceView.CsPricePage
                {
                    ProductName = $"{x.ProductName}( {x.ProductNumber} )",
                    CurrentPrice = "￥ " + x.CurrentPrice.ToDecimal().ToString("N2"),
                    PriceNumber = "￥ " + x.PriceNumber.ToDecimal().ToString("N2"),
                    PriceDate = x.PriceDate.ToDate().ToString("yyyy-M-d HH:mm:ss"),
                    PriceId = x.PriceId,
                    ProductType = ((ProductType)x.ProductType.ToInt()).ToString()
                }),
                total = sh.Total,
                sql = sh.SqlString.ToString()
            });
        }

    }
}