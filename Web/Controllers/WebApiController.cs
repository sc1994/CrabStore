using BLL;
using System;
using System.Linq;
using System.Text;
using Model.DBModel;
using System.Web.Http;
using System.Collections.Generic;
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
        private readonly CsOrderDetailBll _csOrderDetailBll = new CsOrderDetailBll();

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
            strJson.Append("\"priceDate\":\"" + products.FirstOrDefault().OperationDate.ToString("MM-dd") + "\"");
            //大宗采购蟹
            List<CsProducts> product1 = (from product in products
                                         where product.ProductType == 1
                                         select product).ToList();

            strJson.Append(",\"priceList1\":[");
            for (int i = 0; i < (product1.Count / 2); i++)
            {
                strJson.Append("{\"pn1\":\"" + product1[i].ProductName + "\",\"pv1\":" + product1[i].ProductPrice + ",");
                strJson.Append("\"pn2\":\"" + product1[i + 6].ProductName + "\",\"pv2\":" + product1[i + 6].ProductPrice + "}");
                if (i != (product1.Count() / 2 - 1))
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("],\"priceList2\":[");
            //蟹塘采购
            List<CsProducts> product2 = (from product in products
                                         where product.ProductType == 2
                                         select product).ToList();
            for (int j = 0; j < (product2.Count() / 2); j++)
            {
                strJson.Append("{\"pn1\":\"" + product1[j].ProductName + "\",\"pv1\":" + product1[j].ProductPrice + ",");
                strJson.Append("\"pn2\":\"" + product1[j + 6].ProductName + "\",\"pv2\":" + product1[j + 6].ProductPrice + "}");
                if (j != (product2.Count() / 2 - 1))
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
            //获取产品列表
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            //大宗采购公蟹列表
            List<CsProducts> proList1 = (from product1 in productList
                                         where product1.ProductType == 1 && product1.ProductName.StartsWith("公")
                                         select product1).ToList();
            var proList11 = proList1.Select(x => new
            {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number = 0,
                TypeName = "大宗采购"
            });
            //大宗采购母蟹列表
            List<CsProducts> proList2 = (from product2 in productList
                                         where product2.ProductType == 1 && product2.ProductName.StartsWith("母")
                                         select product2).ToList();
            var proList21 = proList2.Select(x => new
            {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number = 0,
                
                TypeName = "大宗采购"
            });
            //蟹唐直采公蟹列表
            List<CsProducts> proList3 = (from product3 in productList
                                         where product3.ProductType == 2 && product3.ProductName.StartsWith("公")
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
                number = 0,
                TypeName = "蟹塘直采"
            });

            //蟹塘直采母蟹列表
            List<CsProducts> proList4 = (from product4 in productList
                                         where product4.ProductType == 2 && product4.ProductName.StartsWith("母")
                                         select product4).ToList();
            var proList41 = proList4.Select(x => new
            {
                x.ProductId,
                x.ProductImage,
                x.ProductPrice,
                x.ProductWeight,
                x.ProductName,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.ProductId, DateTime.Now),
                number = 0,
                TypeName = "蟹塘直采"
            });

            //可选配件列表
            var partList = _csPartsBll.GetModelList(" and PartType=2").Select(x => new
            {
                x.PartId,
                x.PartName,
                x.PartPrice,
                x.PartWeight,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                number = 0,
                TypeName = "可选配件",
                x.PartImage
            }).ToList();

            //必须选配件列表
            var partList1 = _csPartsBll.GetModelList(" and PartType=1").Select(x => new
            {
                x.PartId,
                x.PartName,
                x.PartPrice,
                x.PartWeight,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                number = 0,
                TypeName = "必选配件",
                x.PartImage
            }).ToList();

            return Json(new
            {
                proList1 = proList11,
                proList2 = proList21,
                proList3 = proList31,
                proList4 = proList41,
                partList,
                partList1
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
                List<CsSend> sendList = _csSendBll.GetModelList(" and UserId=" + user.UserId).OrderBy(x => x.IsDefault).ThenBy(x => x.SendId).ToList();
                List<CsAddress> addressList = _csAddressBll.GetModelList(" and UserId=" + user.UserId).OrderBy(x => x.IsDefault).ThenBy(x => x.AddressId).ToList();
                CsDistrictBll disBLL = new CsDistrictBll();
                int firstPrice = 0, fllowPrice = 0;

                if (addressList.Count > 0)
                {
                    string province = addressList[0].Details.Split('&')[0];
                    CsDistrict district = disBLL.GetModel(" Name ='" + province + "'");
                    if (district != null)
                    {
                        firstPrice = district.FirstPrice;
                        fllowPrice = district.FllowPrice;
                    }
                }

                return Json(new
                {
                    status = true,
                    user,
                    sendList,
                    addressList,
                    firstPrice,
                    fllowPrice
                });
            }
            return Json(new
            {
                status = false,
                user = "",
                sendList = "",
                addressList = "",
                firstPrice = 0,
                fllowPrice = 0
            });
        }

        /// <summary>
        /// 根据openid获取该用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserInfo(string openId)
        {
            CsUsers user = _csUsersBll.GetModel(openId);
            if (user != null)
            {
                return Json(new
                {
                    status = true,
                    user
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpPost]
        public IHttpActionResult AddUser(CsUsers user)
        {
            user.UserBalance = 0;
            user.TotalWight = 0;
            user.UserState = 1;
            int number = 0;
            //根据手机号先关联
            CsUsers oldUser = _csUsersBll.GetModelByTelPhone(user.UserPhone);
            if (oldUser != null)
            {
                oldUser.OpenId = user.OpenId;
                bool bl = _csUsersBll.Update(oldUser);
                if (bl)
                {
                    number = oldUser.UserId;
                }
            }else
            {
                number = _csUsersBll.Add(user);
            }
            if (number > 0)
            {
                return Json(new
                {
                    status = true,
                    userid = number
                });
            }
            else
            {

                return Json(new
                {
                    status = false,
                    userid = 0
                });
            }
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
                x.Name,
                x.FirstPrice,
                x.FllowPrice
            }));
        }

        /// <summary>
        /// 添加联系地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAddress(CsAddress address)
        {
            address.CompanyName = "-";
            address.Mobile = "-";
            bool bl = _csAddressBll.ExistsByWhere($" And UserId={address.UserId}");
            address.IsDefault = bl ? 1 : 2;
            int addressId = _csAddressBll.Add(address);
            if (addressId > 0)
            {
                return Json(new
                {
                    status = true,
                    addressid = addressId
                });
            }
            else
            {

                return Json(new
                {
                    status = false,
                    addressid = 0
                });
            }
        }

        /// <summary>
        /// 根据开发编号获取本人订单,分页查询
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public IHttpActionResult GetCsOrderList(string openId,int num,int size)
        {
            int total = 0;
            var list = _csOrderBll.GetModelListByOpenId(openId, num, size, out total);
            return Json(new {
              list= list.Select(x => new {
                    x.OrderId,
                    x.OrderNumber,
                    x.UserId,
                    x.TotalMoney,
                    x.DiscountMoney,
                    x.ActualMoney,
                    OrderDate = x.OrderDate.ToString("yyyy-MM-dd"),
                    OrderState = ((OrderState)x.OrderState).ToString(),
                    consignee = x.OrderAddress.Split('$')[1],
                    telphone = x.OrderAddress.Split('$')[3],
                    address = x.OrderAddress.Split('$')[4],
                    x.SendAddress,
                    x.OrderDelivery,
                    x.OrderCopies,
                    x.TotalWeight,
                    x.BillWeight,
                    x.ExpressMoney,
                    x.ServiceMoney
                }),
                total
            });
        }

        /// <summary>
        /// 添加订单，发起支付
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrder(OrderModel order)
        {
            int orderId = _csOrderBll.AddOrder(order);//获得生成订单编号
            if (orderId > 0)
            {
                return Json(new
                {
                    status = true,
                    orderid = orderId
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    orderid = orderId
                });
            }

        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateOrderState(CsOrder order)
        {

             int number = _csOrderBll.UpdateOrderState(order.OrderId,order.OrderState);            
            //int number = 0;
            if (number > 0)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }

        }

        /// <summary>
        /// 修改预支付信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public IHttpActionResult UpdatePrepaymentId(CsOrder order)
        {
            int number = _csOrderBll.UpdatePrepaymentId(order.OrderId, order.PrepaymentId);
            if (number > 0)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        /// <summary>
        /// 根据订单编号获取订单详情
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public IHttpActionResult GetOrderInfoByOrderId(int id)
        {
            CsOrder order= _csOrderBll.GetModel(id);
            List<CsOrderDetail> orderList = _csOrderDetailBll.GetModelList($" and OrderId={id}");
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            List<CsParts> partList = _csPartsBll.GetModelList("");
            //螃蟹列表
            var crabList = orderList.Where(x=>x.ChoseType==1).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                ProductName =productList.FirstOrDefault(y=>y.ProductId==x.ProductId).ProductName,
                ProductType =((ProductType)productList.FirstOrDefault(y => y.ProductId == x.ProductId).ProductType).ToString(),
                x.ProductNumber,
                x.UnitPrice,
                x.TotalPrice
            });
            //必选配件列表
            var partMustList = orderList.Where(x => x.ChoseType == 2 && x.ProductId < 10005).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                PartName=partList.FirstOrDefault(y=>y.PartId==x.ProductId).PartName,
                PartType="必选配件",
                x.ProductNumber,
                x.UnitPrice,
                x.TotalPrice
            });
            //可选配件列表
            var partOptList = orderList.Where(x => x.ChoseType == 2 && x.ProductId >= 10005).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                PartName = partList.FirstOrDefault(y => y.PartId == x.ProductId).PartName,
                PartType = "配件",
                x.ProductNumber,
                x.UnitPrice,
                x.TotalPrice
            });
            return Json(new {
                order,
                crabList,
                partMustList,
                partOptList
            });
        }

        /// <summary>
        ///根据openId获取用户返利信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public IHttpActionResult GetUserRebateInfo(string openId)
        {
            UserRebateView userRebate = _csUsersBll.GetUserRebateInfo(openId);
            return Json(new {
                userRebate
            });
        }
    }
}
