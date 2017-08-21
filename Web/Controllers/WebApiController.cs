using BLL;
using Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Text;

namespace Web.Controllers
{
    public class WebApiController : ApiController
    {
        private readonly CsPartsBll _csPartsBll = new CsPartsBll();
        private readonly CsProductsBll _csProductsBll = new CsProductsBll();
        private readonly CsPriceBll _csPriceBll = new CsPriceBll();
        private readonly CsOrderBll _csOrderBll = new CsOrderBll();

        [HttpGet]
        public IHttpActionResult GetIndex()
        {
            //获取最新价格            
            var products = new List<CsProducts>();
            products = _csProductsBll.GetModelList("");           
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.Append("\"priceDate\":\""+ products.FirstOrDefault().OperationDate.ToString("yyyy年MM月dd日")+"\"");
            //大宗采购蟹
            List<CsProducts> product1 =( from product in products
                           where product.ProductType == 1
                           select product).ToList();

            strJson.Append(",\"priceList1\":[");
            for (int i = 0; i < (product1.Count() / 2); i++)
            {
                strJson.Append("{\"pn1\":\""+ product1[i].ProductName+"\",\"pv1\":"+ product1[i].ProductPrice+",");
                strJson.Append("\"pn2\":\""+ product1[i+6].ProductName+"\",\"pv2\":"+ product1[i+6].ProductPrice+"}");
                if (i != (product1.Count()/2 - 1))
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("],\"priceList2\":[");
            //蟹塘采购
            List<CsProducts> product2 = (from product in products
                                         where product.ProductType == 2
                                         select product).ToList();
            for(int j = 0; j < (product2.Count() / 2); j++)
            {
                strJson.Append("{\"pn1\":\"" + product1[j].ProductName + "\",\"pv1\":" + product1[j].ProductPrice + ",");
                strJson.Append("\"pn2\":\"" + product1[j + 6].ProductName + "\",\"pv2\":" + product1[j + 6].ProductPrice + "}");
                if (j != (product2.Count()/2 - 1))
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("],\"partsList\":[");
            //获取配件
            var parts = new List<CsParts>();
            parts = _csPartsBll.GetModelList("");
            //必须配件
            List<CsParts> parts1 = (from part in parts
                                    where part.PartType == 1
                                    select part).ToList();
            //可选配件
            List<CsParts> parts2 = (from part in parts
                                    where part.PartType == 2
                                    select part).ToList();
            int number1 = parts1.Count();
            int number2 = parts2.Count();
            int number = number1 > number2 ? number1 : number2;
            for (var n=0;n<number;n++)
            {
                strJson.Append("{");
                if (n < number1)
                {
                    strJson.Append("\"pr1\":\""+parts1[n].PartName+"\",\"pv1\":"+parts1[n].PartPrice+",");
                }
                else
                {
                    strJson.Append("\"pr1\":\"\",\"pv1\":\"\",");
                }
                if (n < number2)
                {
                    strJson.Append("\"pr2\":\""+parts2[n].PartName+"\",\"pv2\":"+parts2[n].PartPrice);
                }else
                {
                    strJson.Append("\"pr2\":\"\",\"pv2\":\"\"");
                }
                strJson.Append("}");
                if (n < (number - 1))
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("]");
            strJson.Append("}");
            return Json(strJson.ToString());
        }

        [HttpGet]
        public IHttpActionResult GetProductList()
        {
            //获取产品列表
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            //大宗采购公蟹列表
            List<CsProducts> proList1 = (from product1 in productList
                                         where product1.ProductType == 1&& product1.ProductName.StartsWith("公")
                                         select product1).ToList();
            var proList11 =proList1.Select(x=>new {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate =x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber =_csOrderBll.TotalNumber(x.ProductId,DateTime.Now),
                number=0
            });
            //大宗采购母蟹列表
            List<CsProducts> proList2 = (from product2 in productList
                                         where product2.ProductType == 1 && product2.ProductName.StartsWith("母")
                                        select product2).ToList();
            var proList21 = proList2.Select(x=>new {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number=0
            });
            //蟹唐直采公蟹列表
            List<CsProducts> proList3 = (from product3 in productList
                                         where product3.ProductType == 2 &&product3.ProductName.StartsWith("公")
                                         select product3).ToList();
            var proList31 = proList3.Select(x => new
            {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number=0
            });

            //蟹塘直采母蟹列表
            List<CsProducts> proList4 = (from product4 in productList
                                         where product4.ProductType == 2 && product4.ProductName.StartsWith("母")
                                         select product4).ToList();
            var proList41 = proList4.Select(x=>new {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number=0
            });
            //可选配件列表
            var partList = _csPartsBll.GetModelList(" and PartType=2").Select(x=>new {
                x.PartId,
                x.PartName,
                x.PartPrice,
                x.PartWeight,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
            }).ToList();

            return Json(new {
                proList1=proList11,
                proList2 = proList21,
                proList3 = proList31,
                proList4 =proList41,
                partList = partList

            });
        }
    }
}
