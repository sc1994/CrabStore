using BLL;
using System;
using System.Linq;
using System.Text;
using Model.DBModel;
using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Common;
using Model.ViewModel;

namespace Web.Controllers
{
    public class WebApiController : ApiController
    {
        private readonly CsSendBll _csSendBll = new CsSendBll();
        private readonly CsPartsBll _csPartsBll = new CsPartsBll();
        private readonly CsOrderBll _csOrderBll = new CsOrderBll();
        private readonly CsUsersBll _csUsersBll = new CsUsersBll();
        private readonly CsAddressBll _csAddressBll = new CsAddressBll();
        private readonly CsProductsBll _csProductsBll = new CsProductsBll();

        /// <summary>
        /// 获取螃蟹价格表和配件价格表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetIndex()
        {
            //获取最新价格            
            var products = new List<CsProducts>();
            products = _csProductsBll.GetModelList("");
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.Append("\"priceDate\":\"" + products.FirstOrDefault()?.OperationDate.ToString("yyyy年MM月dd日") + "\"");
            //大宗采购蟹
            List<CsProducts> product1 = (from product in products
                                         where product.ProductType == 1
                                         select product).ToList();

            strJson.Append(",\"priceList1\":[");
            for (int i = 0; i < (product1.Count / 2); i++)
            {
                strJson.Append("{\"pn1\":\"" + product1[i].ProductName + "\",\"pv1\":" + product1[i].ProductPrice + ",");
                strJson.Append("\"pn2\":\"" + product1[i + 6].ProductName + "\",\"pv2\":" + product1[i + 6].ProductPrice + "}");
                if (i != (product1.Count / 2 - 1))
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("],\"priceList2\":[");
            //蟹塘采购
            List<CsProducts> product2 = (from product in products
                                         where product.ProductType == 2
                                         select product).ToList();
            for (int j = 0; j < (product2.Count / 2); j++)
            {
                strJson.Append("{\"pn1\":\"" + product1[j].ProductName + "\",\"pv1\":" + product1[j].ProductPrice + ",");
                strJson.Append("\"pn2\":\"" + product1[j + 6].ProductName + "\",\"pv2\":" + product1[j + 6].ProductPrice + "}");
                if (j != (product2.Count / 2 - 1))
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
            int number1 = parts1.Count;
            int number2 = parts2.Count;
            int number = number1 > number2 ? number1 : number2;
            for (var n = 0; n < number; n++)
            {
                strJson.Append("{");
                if (n < number1)
                {
                    strJson.Append("\"pr1\":\"" + parts1[n].PartName + "\",\"pv1\":" + parts1[n].PartPrice + ",");
                }
                else
                {
                    strJson.Append("\"pr1\":\"\",\"pv1\":\"\",");
                }
                if (n < number2)
                {
                    strJson.Append("\"pr2\":\"" + parts2[n].PartName + "\",\"pv2\":" + parts2[n].PartPrice);
                }
                else
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

        /// <summary>
        /// 获取螃蟹产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProductList()
        {
            // 获取产品列表
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            var totalList = _csOrderBll.TotalNumber(string.Join(",", productList.Select(x => x.ProductId)));

            //大宗采购公蟹列表
            var proList1 = productList
                .Where(x => x.ProductType == 1 && x.ProductName.StartsWith("公"))
                .Select(x => GetProductModelView(x, totalList));

            // 大宗采购母蟹列表
            var proList2 = productList
                .Where(x => x.ProductType == 1 && x.ProductName.StartsWith("母"))
                .Select(x => GetProductModelView(x, totalList));

            // 蟹唐直采公蟹列表
            var proList3 = productList
                .Where(x => x.ProductType == 2 && x.ProductName.StartsWith("公"))
                .Select(x => GetProductModelView(x, totalList));

            // 蟹塘直采母蟹列表
            var proList4 = productList
                .Where(x => x.ProductType == 2 && x.ProductName.StartsWith("母"))
                .Select(x => GetProductModelView(x, totalList));

            // 配件列表
            var partList = _csPartsBll.GetModelList("").Select(x => new
            {
                x.PartId,
                x.PartName,
                x.PartPrice,
                x.PartWeight,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                number = 0,
                TypeName = ((PartType)x.PartType).ToString(),
                x.PartType
            });

            return Json(new
            {
                proList1,
                proList2,
                proList3,
                proList4,
                partList = partList.Where(x => x.PartType == PartType.可选配件.GetHashCode()),
                partList1 = partList.Where(x => x.PartType == PartType.必选配件.GetHashCode())
            });
        }

        /// <summary>
        /// 获取配件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPartList(int id)
        {
            //可选配件列表
            var partList = _csPartsBll.GetModelList(" and PartType=" + id).Select(x => new
            {
                x.PartId,
                x.PartName,
                x.PartPrice,
                x.PartWeight,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                number = 0,
                TypeName = id == 1 ? "必须配件" : "可选配件"
            }).ToList();
            return Json(partList);
        }

        /// <summary>
        /// 根据openId查询该用户的发货信息和收货信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAddress(string openId)
        {
            CsUsers user = _csUsersBll.GetModel(openId);
            if (user != null)
            {
                //根据userId查询出发件信息和收获地址信息
                List<CsSend> sendList = _csSendBll.GetModelList(" and UserId=" + user.UserId);
                List<CsAddress> addressList = _csAddressBll.GetModelList(" and UserId=" + user.UserId);
                return Json(new
                {
                    status = true,
                    sendList,
                    addressList
                });
            }
            return Json(new
            {
                status = false,
                sendList = "",
                addressList = ""
            });
        }

        [HttpPost]
        public IHttpActionResult AddUser(CsUsers user)
        {
            user.UserBalance = 0;
            user.TotalWight = 0;
            user.UserState = 1;

            int number = _csUsersBll.Add(user);
            if (number > 0)
            {
                return Json(new
                {
                    status = true,
                    userid = number
                });
            }

            return Json(new
            {
                status = false,
                userid = 0
            });
        }

        [HttpGet]
        public IHttpActionResult GetCityList(int parentId = 0)
        {
            var csDistrictBll = new CsDistrictBll();
            var list = csDistrictBll.GetModelList($" AND ParentId = {parentId} ").OrderBy(x => x.Sort);
            return Json(list.Select(x => new
            {
                x.ParentId,
                x.Id,
                x.Name
            }));
        }

        public IHttpActionResult GetCsOrderList(int userId = 0)
        {
            if (userId < 1)
            {
                return Json(new
                {
                    code = ResStatue.No,
                    data = "缺少必要的参数"
                });
            }
            var list = _csOrderBll.GetModelList($" AND UserId = {userId} AND RowStatus = {RowStatus.有效.GetHashCode()} ");
            return Json(new
            {
                code = ResStatue.Yes,
                data = list
            });
        }

        /// <summary>
        /// 获取 Product 视图对应实体
        /// </summary>
        /// <param name="x"></param>
        /// <param name="totalList"></param>
        /// <returns></returns>
        private dynamic GetProductModelView(CsProducts x, IEnumerable<CsOrderView.CsOrderTotalByProduct> totalList)
            => new
            {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = totalList.FirstOrDefault(t => t.ProductId == x.ProductId)?.Total ?? 0,
                number = 0,
                TypeName = ((ProductType)x.ProductType).ToString()
            };
    }
}
