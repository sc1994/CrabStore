﻿using BLL;
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
            strJson.Append("\"priceDate\":\"" + DateTime.Now.ToString("MM-dd") + "\"");
            //大宗采购蟹
            //List<CsProducts> product1 = (from product in products
            //                             where product.ProductType == 1
            //                             select product).ToList();

            //strJson.Append(",\"priceList1\":[");
            //for (int i = 0; i < (product1.Count / 2); i++)
            //{
            //    strJson.Append("{\"pn1\":\"" + product1[i].ProductName + "\",\"pv1\":" + (product1[i].ProductState == 1 ? product1[i].ProductPrice.ToString() : "\"-\"") + ",");
            //    strJson.Append("\"pn2\":\"" + product1[i + 6].ProductName + "\",\"pv2\":" + (product1[i + 6].ProductState == 1 ? product1[i + 6].ProductPrice.ToString() : "\"-\"") + "}");
            //    if (i != (product1.Count() / 2 - 1))
            //    {
            //        strJson.Append(",");
            //    }
            //}
            //strJson.Append("],");
            strJson.Append(",\"priceList2\":[");
            //蟹塘采购
            //List<CsProducts> product2 = (from product in products
            //                             where product.ProductType == 2
            //                             select product).ToList();
            List<CsProducts> product2 = products.Where(x => x.ProductType == 2).ToList();
            for (int j = 0; j < (product2.Count() / 2); j++)
            {
                strJson.Append("{\"pn1\":\"" + product2[j].ProductName + "\",\"pv1\":" + (product2[j].ProductState == 1 ? product2[j].ProductPrice.ToString() : "\"-\"") + ",");
                strJson.Append("\"pn2\":\"" + product2[j + 6].ProductName + "\",\"pv2\":" + (product2[j + 6].ProductState == 1 ? product2[j + 6].ProductPrice.ToString() : "\"-\"") + "}");
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
            List<CsParts> parts1 = parts.Where(x => x.PartType == 1).ToList();
            //可选配件
            List<CsParts> parts2 = parts.Where(x => x.PartType == 2).ToList();
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
                x.ProductStock,//库存
                TypeName = "大宗采购",
                x.ProductState//库存状态
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
                x.ProductStock,//库存
                TypeName = "大宗采购",
                x.ProductState,//库存状态
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
                x.ProductStock,//库存
                TypeName = "蟹塘直采",
                x.ProductState//库存状态
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
                x.ProductStock,//库存
                TypeName = "蟹塘直采",
                x.ProductState//库存状态
            });

            //可选配件列表
            var partList = _csPartsBll.GetModelList(" and PartType=2 and PartState=1 ").Select(x => new
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
            var partList1 = _csPartsBll.GetModelList(" and PartType=1 and PartState=1 ").Select(x => new
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
            var partList = _csPartsBll.GetModelList(" and PartState=1 and PartType=" + id).Select(x => new
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
        /// 获取套餐列表
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetPackageList()
        {
            CsPackageBll packageBll = new CsPackageBll();
            var packAgeList = packageBll.GetModelList(" and PackageState=1 ").Select(x => new
            {
                x.PackageId,
                x.PackageNumber,
                x.PackageType,
                TypeName = ((ProductType)x.PackageType).ToString(),
                x.PackageName,
                x.PackageImage,
                x.PackageWeight,
                x.PackagePrice,
                x.PackageState,
                OperationDate = x.OperationDate.ToString("yyyy-MM-dd"),
                TotalNumber = _csOrderBll.TotalNumber(x.PackageId, DateTime.Now),
                number = 0,
                x.PackageStock
            }).ToList();
            if (packAgeList.Count > 0)
            {
                return Json(new
                {
                    status = true,
                    packagelist = packAgeList
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
                List<CsAddress> addressList = _csAddressBll.GetModelList(" and UserId=" + user.UserId + " and AddressState=1 ").OrderBy(x => x.IsDefault).ThenBy(x => x.AddressId).ToList();
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
            LogHelper.Log("public IHttpActionResult GetAddress-------openId:" + openId, "openId 未能查询到用户信息");
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

        [HttpGet]
        public IHttpActionResult GetAddressList(int userId)
        {
            List<CsAddress> addressList = _csAddressBll.GetModelList($" and UserId={userId} and AddressState=1 ");
            CsDistrictBll districtBll = new CsDistrictBll();
            //所有省会列表 获取对于首发总量价格与续重价格
            List<CsDistrict> districtList = districtBll.GetModelList(" and ParentId=0");
            var list = addressList.Select(x => new
            {
                x.AddressId,
                x.Consignee,
                x.TelPhone,
                x.ConSex,
                Province = x.Details.Split('&')[0],
                City = x.Details.Split('&')[1],
                District = x.Details.Split('&')[2],
                Detail = x.Details.Split('&')[3],
                FirstPrice = districtList.FirstOrDefault(y => y.Name == x.Details.Split('&')[0])?.FirstPrice ?? 12,
                FllowPrice = districtList.FirstOrDefault(y => y.Name == x.Details.Split('&')[0])?.FllowPrice ?? 2,
                x.IsDefault
            });
            return Json(new
            {
                status = true,
                list
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
            LogHelper.Log("public IHttpActionResult GetAddress", "openId 未能查询到用户信息");
            return Json(new
            {
                status = false
            });
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
            }
            else
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
            address.AddressState = 1;
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
        public IHttpActionResult GetCsOrderList(string openId, int num, int size)
        {
            int total = 0;
            var list = _csOrderBll.GetModelListByOpenId(openId, num, size, out total);
            return Json(new
            {
                list = list.Select(x => new
                {
                    x.OrderId,
                    x.OrderNumber,
                    x.UserId,
                    x.TotalMoney,
                    x.DiscountMoney,
                    x.ActualMoney,
                    OrderDate = x.OrderDate.ToString("yyyy-MM-dd"),
                    OrderStateStr = ((OrderState)x.OrderState).ToString(),
                    consignee = x.OrderAddress.Split('$')[1],
                    telphone = x.OrderAddress.Split('$')[3],
                    address = x.OrderAddress.Split('$')[4],
                    x.SendAddress,
                    x.OrderDelivery,
                    x.OrderCopies,
                    x.TotalWeight,
                    x.BillWeight,
                    x.ExpressMoney,
                    x.ServiceMoney,
                    x.PrepaymentId,
                    x.OrderState
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
            //try
            //{
            //    LogHelper.Log("生成订单"+order.ToJson(),"订单测试");
                string orderNumber = "";
                int orderId = _csOrderBll.AddOrder(order, out orderNumber);//获得生成订单编号
                if (orderId > 0)
                {
                    return Json(new
                    {
                        status = true,
                        orderid = orderId,
                        orderNumber
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        orderid = orderId,
                        orderNumber
                    });
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Log(ex.Message,"订单生成异常");
            //    return Json(new
            //                {
            //                    status = false,
            //                    orderid = 0,
            //                    orderNumber=""
            //                });
            //}

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

            int number = _csOrderBll.UpdateOrderState(order.OrderId, order.OrderState);
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

        public IHttpActionResult FinshOrder(CsOrder order)
        {
            int number = _csOrderBll.FinshOrder(order.OrderId, order.UserId, order.TotalWeight, order.OrderCopies);
            if (number > 0)
            {
                return Json(new
                {
                    status = true
                });
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
            CsOrder order = _csOrderBll.GetModel(id);
            List<CsOrderDetail> orderList = _csOrderDetailBll.GetModelList($" and OrderId={id}");
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            List<CsParts> partList = _csPartsBll.GetModelList("");
            CsPackageBll _cspackageBll = new CsPackageBll();
            List<CsPackage> packageList = _cspackageBll.GetModelList("");
            //螃蟹列表
            var crabList = orderList.Where(x => x.ChoseType == 1).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                ProductName = productList.FirstOrDefault(y => y.ProductId == x.ProductId).ProductName,
                ProductType = ((ProductType)productList.FirstOrDefault(y => y.ProductId == x.ProductId).ProductType).ToString(),
                ProductWeight = (ProductType)productList.FirstOrDefault(y => y.ProductId == x.ProductId).ProductWeight,
                x.ProductNumber,
                x.UnitPrice,
                x.TotalPrice
            });
            //必选配件列表
            var partMustList = orderList.Where(x => x.ChoseType == 2 && x.ProductId < 10005 && x.ProductId > 10000).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                PartName = partList.FirstOrDefault(y => y.PartId == x.ProductId).PartName,
                PartType = "必选配件",
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
            //套餐列表
            var packList = orderList.Where(x => x.ChoseType == 3).Select(x => new
            {
                x.OrderId,
                x.ProductId,
                ProductName = packageList.FirstOrDefault(y => y.PackageId == x.ProductId).PackageName,
                ProductType = ((ProductType)packageList.FirstOrDefault(y => y.PackageId == x.ProductId).PackageType).ToString(),
                ProductWeight = (ProductType)packageList.FirstOrDefault(y => y.PackageId == x.ProductId).PackageWeight,
                x.ProductNumber,
                x.UnitPrice,
                x.TotalPrice
            });
            return Json(new
            {
                order,
                crabList,
                partMustList,
                partOptList,
                packList
            });
        }

        /// <summary>
        /// 修改用户默认收货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ChooseDefault([FromBody] int addressId)
        {
            CsAddress address = _csAddressBll.GetModel(addressId);
            int number = _csAddressBll.ChooseAddress(address);
            bool status = false;
            if (number > 0)
            {
                status = true;
            }

            return Json(new { status });
        }

        /// <summary>
        /// 删除收获地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteAddress([FromBody] int id)
        {

            bool status = _csAddressBll.UpdateState(id);
            return Json(status);
        }

        /// <summary>
        /// 完成订单支付2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FinshOrder2([FromBody] int orderId)
        {
            CsOrder order = _csOrderBll.GetModel(orderId);
            List<CsProducts> productList = _csProductsBll.GetModelList("");
            var proList = _csOrderDetailBll.GetModelList(" and OrderId=" + orderId + " and ChoseType=1").Select(x => new
            {
                x.ProductId,
                ProductWeight = productList.FirstOrDefault(y => y.ProductId == x.ProductId).ProductWeight * x.ProductNumber
            });
            decimal totalWeight = 0;
            foreach (var pro in proList)
            {
                totalWeight += pro.ProductWeight;
            }

            int number = _csOrderBll.FinshOrder(order.OrderId, order.UserId, totalWeight, order.OrderCopies);
            if (number > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        #region 套餐订单操作
        /// <summary>
        /// 添加套餐订单
        /// </summary>
        /// <param name="order">套餐订单</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPackageOrder(OrderModel order)
        {
            string orderNumber = "";
            int orderId = _csOrderBll.AddPackageOrder(order, out orderNumber);//获得生成订单编号
            if (orderId > 0)
            {
                return Json(new
                {
                    status = true,
                    orderid = orderId,
                    orderNumber
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    orderid = orderId,
                    orderNumber
                });
            }
        }

        /// <summary>
        /// 完成套餐订单支付操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FinshPackageOrder([FromBody] int orderId)
        {
            int number = _csOrderBll.FinshPackageOrder(orderId);
            if (number > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new { status = false });
            }
        }
        #endregion
    }
}
