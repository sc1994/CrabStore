using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly CsOrderBll _csOrderBll = new CsOrderBll();
        private readonly CsOrderDetailBll _csOrderDetailBll = new CsOrderDetailBll();
        private readonly CsPartsBll _csPartsBll = new CsPartsBll();
        private readonly CsProductsBll _csProductsBll = new CsProductsBll();
        private readonly CsUsersBll _csUsersBll = new CsUsersBll();

        // GET: CsOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsOrderPage(CsOrderView.CsOrderWhere para)
        {
            var data = GetList(para);

            var list = data.Data.Select(x => new
            {
                x.OrderId,
                x.OrderNumber,
                OrderDate = x.OrderDate.ToString("yyyy-M-d HH:mm:ss"),
                OrderState = ((OrderState)x.OrderState).ToString(),
                RowStatus = ((RowStatus)x.RowStatus).ToString(),
                DeleteDate = x.DeleteDate.ToString("yyyy-M-d HH:mm:ss"),
                x.DeleteDescribe,
                UserName = x.UserName + $"({x.UserSex}) / " + x.UserId,
                x.UserPhone,
                TotalMoney = "￥" + x.TotalMoney.ToString("N2")
            });

            return Json(new
            {
                data = list,
                sql = data.Sql,
                total = data.Total
            });
        }

        public ActionResult GetCsOrderInfo(int id)
        {
            var csOrder = _csOrderBll.GetModel(id);
            var csOrderDetails = _csOrderDetailBll.GetModelList(" AND OrderId = " + id);
            var user = _csUsersBll.GetModel(csOrder.UserId);

            var parts = new List<CsParts>();
            var products = new List<CsProducts>();
            #region 相关配件和蟹
            if (csOrderDetails.Any(x => x.ChoseType == ChoseType.配件.GetHashCode()))
            {
                parts = _csPartsBll.GetModelList($" AND PartId IN ({string.Join(",", csOrderDetails.Where(x => x.ChoseType == ChoseType.配件.GetHashCode()).Select(x => x.ProductId))})");
            }
            if (csOrderDetails.Any(x => x.ChoseType == ChoseType.螃蟹.GetHashCode()))
            {
                products = _csProductsBll.GetModelList($" AND ProductId IN ({string.Join(",", csOrderDetails.Where(x => x.ChoseType == ChoseType.螃蟹.GetHashCode()).Select(x => x.ProductId))})");
            }
            #endregion

            var csOrderDetailExtends = new List<CsOrderView.CsOrderDetailExtend>();
            foreach (var csOrderDetail in csOrderDetails)
            {
                var csOrderDetailExtend = new CsOrderView.CsOrderDetailExtend
                {
                    ChoseType = ((ChoseType)csOrderDetail.ChoseType).ToString(),
                    DetailId = csOrderDetail.DetailId,
                    ProductId = csOrderDetail.ProductId,
                    ProductNumber = csOrderDetail.ProductNumber,
                    TotalPrice = "￥" + csOrderDetail.TotalPrice.ToString("N2"),
                    UnitPrice = "￥" + csOrderDetail.UnitPrice.ToString("N2")
                };
                // 计算商品名称
                if (csOrderDetailExtend.ChoseType == ChoseType.配件.ToString())
                {
                    csOrderDetailExtend.ProductName = $"配件/{parts.FirstOrDefault(x => x.PartId == csOrderDetailExtend.ProductId)?.PartName ?? "暂无名称"}({csOrderDetailExtend.ProductId})";
                }
                else
                {
                    var product = products.FirstOrDefault(x => x.ProductId == csOrderDetailExtend.ProductId);
                    if (product != null)
                    {
                        csOrderDetailExtend.ProductName = $"{(ProductType)product.ProductType}/{product.ProductName}({csOrderDetailExtend.ProductId})";
                    }
                    else
                    {
                        csOrderDetailExtend.ProductName = "暂无名称";
                    }
                }
                csOrderDetailExtends.Add(csOrderDetailExtend);
            }

            return Json(new CsOrderView.CsOrderInfo
            {
                RowStatus = csOrder.RowStatus.ToString(),
                OrderState = csOrder.OrderState.ToString(),
                RowStatusDescribe = ((RowStatus)csOrder.RowStatus).ToString(),
                OrderStateDescribe = ((OrderState)csOrder.OrderState).ToString(),
                OrderId = csOrder.OrderId,
                OrderNumber = csOrder.OrderNumber,
                OrderDate = csOrder.OrderDate.ToString("yyyy-M-d HH:mm:ss"),
                DeleteDate = csOrder.DeleteDate.ToString("yyyy-M-d HH:mm:ss"),
                DeleteDescribe = csOrder.DeleteDescribe,
                TotalMoney = "￥" + csOrder.TotalMoney.ToString("N2"),
                ActualMoney = "￥" + csOrder.ActualMoney.ToString("N2"),
                DiscountMoney = "￥" + csOrder.DiscountMoney.ToString("N2"),
                UserName = $"{user.UserName}({user.UserSex})",
                UserPhone = user.UserPhone,
                CsOrderDetails = csOrderDetailExtends,
                OrderDelivery = csOrder.OrderDelivery,
                OrderAddress = csOrder.OrderAddress
            });
        }

        public ActionResult UpdateCsOrder(int id,
                                          int rowStatus,
                                          DateTime deleteDate,
                                          string deleteDescribe,
                                          int orderState,
                                          string delivery)
        {
            if (id < 1)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "缺少必要的Id"
                });
            }

            var model = _csOrderBll.GetModel(id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "Id 未能查询到对应订单, 请刷新页面再试"
                });
            }
            model.RowStatus = rowStatus;
            if (rowStatus == RowStatus.无效.GetHashCode())
            {
                model.DeleteDate = deleteDate;
                model.DeleteDescribe = deleteDescribe;
            }
            else
            {
                model.DeleteDate = "1900-1-1".ToDate();
                model.DeleteDescribe = "";
            }
            model.OrderState = orderState;
            if (orderState == OrderState.已发货.GetHashCode())
            {
                model.OrderDelivery = delivery;
            }
            else
            {
                model.OrderDelivery = "";
            }
            var line = _csOrderBll.Update(model);

            if (line)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.Yes,
                    Data = "更新成功"
                });
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.Warn,
                Data = "更新成功,但是没有受影响行数"
            });
        }

        public ActionResult ExportCsOrder(CsOrderView.CsOrderWhere para)
        {
            var data = GetList(para, false);
            var details = _csOrderDetailBll.GetModelList($" AND OrderId IN ({string.Join(",", data.Data.Select(x => x.OrderId))})");
            var products = _csProductsBll.GetModelList("");
            var parts = _csPartsBll.GetModelList("");
            var list = new List<CsOrderView.CsOrderExcel>();
            foreach (var order in data.Data)
            {
                var item = new CsOrderView.CsOrderExcel
                {
                    用户订单号 = order.OrderNumber,
                };

                //var detailWheres = details.Where(x => x.OrderId == order.OrderId);
                //foreach (var detail in detailWheres)
                //{
                //    var product = products.FirstOrDefault(x => x.ProductId == detail.ProductId);
                //    var part = parts.FirstOrDefault(x => x.PartId == detail.ProductId);
                //    if (product == null && detail.ChoseType == ChoseType.螃蟹.GetHashCode())
                //    {
                //        continue;
                //    }
                //    if (part == null && detail.ChoseType == ChoseType.配件.GetHashCode())
                //    {
                //        continue;
                //    }
                //    var isFirst = detailWheres.ToList().IndexOf(detail) == 0;
                //    if (!isFirst)
                //    {
                //        item = new CsOrderView.CsOrderExcel
                //        {
                //            商品名称 = detail.ChoseType == ChoseType.螃蟹.GetHashCode() ? product?.ProductName : part?.PartName,
                //            数量 = detail.ProductNumber.ToString(),
                //            // ReSharper disable once PossibleNullReferenceException
                //            种类 = detail.ChoseType == ChoseType.螃蟹.GetHashCode() ? ((ProductType)product.ProductType).ToString() : ((PartType)part.PartType).ToString(),
                //        };
                //        list.Add(item);
                //    }
                //    else
                //    {
                //        item.商品名称 = detail.ChoseType == ChoseType.螃蟹.GetHashCode() ? product?.ProductName : part?.PartName;
                //        item.数量 = detail.ProductNumber.ToString();
                //        // ReSharper disable once PossibleNullReferenceException
                //        item.种类 = detail.ChoseType == ChoseType.螃蟹.GetHashCode() ? ((ProductType)product.ProductType).ToString() : ((PartType)part.PartType).ToString();
                //        list.Add(item);
                //    }
                //}
                // 空行 
                //list.Add(new CsOrderView.CsOrderExcel());
            }
            var path = $"excel/{DateTime.Now:yyyyMMddHHmmssffff}.xls";
            try
            {
                NpoiHelper.ExportToExcel(list.ToDataTable(), AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path);
            }
            catch (Exception e)
            {
                return Json(new ResModel
                {
                    Data = e.Message,
                    ResStatus = ResStatue.No
                });
            }
            return Json(new ResModel
            {
                Data = "../" + path,
                ResStatus = ResStatue.Yes
            });
        }

        public ActionResult ImportCsOrder(string path)
        {
            path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path.Replace("..", "");
            var fileArr = path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = fileArr[fileArr.Length - 1];
            var orders = NpoiHelper.ReadExcel(path.Replace("/" + fileName, ""), fileName).ToList<CsOrderView.CsOrderImport>();
            if (!orders.Any())
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "当前Excel中没有数据,请确认文件是否包含规定完整的数据"
                });
            }
            var users = _csUsersBll.GetModelList($" AND (UserName IN ('{string.Join("','", orders.Select(x => x.收货人))}') OR UserPhone IN ('{string.Join("','", orders.Select(x => x.联系电话))}'))");
            var products = _csProductsBll.GetModelList("");
            var parts = _csPartsBll.GetModelList("");
            var data = new List<CsOrderView.CsOrderAndDetail>();
            var item = new CsOrderView.CsOrderAndDetail();
            foreach (var order in orders)
            {
                var type = ExcelRowType(order);
                if (type == ExcelRow.Other)
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.No,
                        Data = "当前Excel中不满足格式的单元格,请仔细确认数据, 数据一旦上传将无法撤回"
                    });
                }
                if (type == ExcelRow.Empty)
                {
                    data.Add(item);
                    item = new CsOrderView.CsOrderAndDetail();
                    continue;
                }
                var product = products.FirstOrDefault(x => x.ProductName == order.商品名称 && ((ProductType)x.ProductType).ToString() == order.种类);
                var part = parts.FirstOrDefault(x => x.PartName == order.商品名称 && ((PartType)x.PartType).ToString() == order.种类);
                var pId = 0; // 商品Id
                var cType = ChoseType.螃蟹; // 蟹或配件
                if (product == null && part == null)
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.No,
                        Data = $"商品名称{order.商品名称}/种类{ order.种类},不存在与数据库中,请仔细确认数据"
                    });
                }
                if (product != null)
                {
                    pId = product.ProductId;
                    cType = ChoseType.螃蟹;
                }
                if (part != null)
                {
                    pId = part.PartId;
                    cType = ChoseType.配件;
                }
                if (type == ExcelRow.Order)
                {
                    var user = users.FirstOrDefault(x => x.UserPhone == order.联系电话 && x.UserName == order.收货人);
                    int userId;
                    if (user == null)
                    {
                        userId = _csUsersBll.Add(new CsUsers
                        {
                            UserSex = "先生",
                            UserName = order.收货人,
                            UserPhone = order.联系电话,
                            OpenId = "",
                            Remarks = "",
                            TotalWight = 0,
                            UserBalance = 0,
                            UserState = 1
                        });
                    }
                    else
                    {
                        userId = user.UserId;
                    }
                    Thread.Sleep(5);
                    item.CsOrder = new CsOrder
                    {
                        RowStatus = RowStatus.有效.GetHashCode(),
                        DeleteDescribe = "",
                        OrderState = OrderState.已发货.GetHashCode(),
                        UserId = userId,
                        ActualMoney = order.实收金额.ToDecimal(),
                        OrderDelivery = order.货运单号,
                        OrderAddress = order.收货地址,
                        DeleteDate = "1900-1-1".ToDate(),
                        DiscountMoney = order.总金额.ToDecimal() - order.实收金额.ToDecimal(),
                        OrderDate = DateTime.Now,
                        OrderNumber = DateTime.Now.ToString(OrderNumberFormat),
                        TotalMoney = order.总金额.ToDecimal()
                    };
                    item.CsOrderDetails.Add(new CsOrderDetail
                    {
                        ProductId = pId,
                        ProductNumber = order.数量.ToInt(),
                        ChoseType = cType.GetHashCode(),
                        TotalPrice = order.单价.ToDecimal() * order.数量.ToInt(),
                        UnitPrice = order.单价.ToDecimal()
                    });
                }
                if (type == ExcelRow.Detail)
                {
                    item.CsOrderDetails.Add(new CsOrderDetail
                    {
                        ProductId = pId,
                        ProductNumber = order.数量.ToInt(),
                        ChoseType = cType.GetHashCode(),
                        TotalPrice = order.单价.ToDecimal() * order.数量.ToInt(),
                        UnitPrice = order.单价.ToDecimal()
                    });
                }
            }

            var count = 0;
            var countDetail = 0;
            foreach (var d in data)
            {
                var orderId = _csOrderBll.Add(d.CsOrder);
                foreach (var detail in d.CsOrderDetails)
                {
                    detail.OrderId = orderId;
                    _csOrderDetailBll.Add(detail);
                    countDetail++;
                }
                count++;
            }

            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = $"导入{count}条订单记录,以及共计{countDetail}条商品记录"
            });
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistic()
        {
            return View();
        }

        public ActionResult GetStatisticList(StatisticView.StatisticWhere para)
        {
            #region 获取符合条件商品
            var shProducts = new SqlHelper<CsProducts>();
            if (!para.ProductName.IsNullOrEmpty())
                shProducts.AddWhere(CsProductsEnum.ProductName, para.ProductName, RelationEnum.Like);
            if (para.ProductType != 0)
                shProducts.AddWhere(CsProductsEnum.ProductType, para.ProductType);
            var products = shProducts.Select();
            if (!products.Any())
            {
                return Json(new
                {
                    data = new ArrayList(),
                    msg = "没有满足当前条件的商品"
                });
            }

            #endregion

            #region 获取未发货订单数量
            var sh = new SqlHelper<StatisticView.StatisticList>("CsOrderDetail")
            {
                Alia = "cod"
            };
            sh.AddJoin(JoinEnum.LeftJoin, "CsOrder", "co", "OrderId", "OrderId", $" AND ProductId IN ({string.Join(",", products.Select(x => x.ProductId))})");
            sh.AddShow("OrderState,ProductId,co.OrderId,OrderDate,ProductNumber,ChoseType");
            sh.AddWhere($" AND OrderState IN ({OrderState.支付成功.GetHashCode()},{OrderState.配货中.GetHashCode()})");
            var orders = sh.Select();
            #endregion

            #region 整合数据
            var data = new List<StatisticView.StatisticList>();
            foreach (var product in products)
            {
                var item = new StatisticView.StatisticList
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductType = ((ProductType)product.ProductType).ToString(),
                };
                var total = orders
                    .Where(x => x.ProductId == product.ProductId)
                    .Sum(x => x.ProductNumber);
                item.Total = $"{total} 只 / 计 {total * product.ProductWeight} 斤";
                var stock = orders
                    .Where(x => x.ProductId == product.ProductId && x.OrderState == OrderState.配货中.GetHashCode())
                    .Sum(x => x.ProductNumber);
                item.Stock = $"{stock} 只 / 计 {stock * product.ProductWeight} 斤";
                var unStork = orders
                    .Where(x => x.ProductId == product.ProductId && x.OrderState == OrderState.支付成功.GetHashCode())
                    .Sum(x => x.ProductNumber);
                item.UnStork = $"{unStork} 只 / 计 {unStork * product.ProductWeight} 斤";
                item.UnStorkInt = unStork;
                data.Add(item);
            }

            #endregion

            return Json(data.OrderByDescending(x => x.UnStorkInt));
        }

        /// <summary>
        /// 获取当前条件的列表(用于分页查询和导出)
        /// </summary>
        /// <param name="para">条件参数</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        private PageInfo<CsOrderView.CsOrderPage> GetList(CsOrderView.CsOrderWhere para, bool isPage = true)
        {
            var sh = new SqlHelper<CsOrderView.CsOrderPage>("CsOrder")
            {
                PageConfig = new PageConfig
                {
                    PageIndex = isPage ? para.CurrentPage : 0,
                    PageSize = isPage ? PageSize : 0,
                    PageSortField = CsOrderEnum.OrderId.ToString(),
                    SortEnum = SortEnum.Desc
                },
                Alia = "co"
            };
            sh.AddShow(CsOrderEnum.OrderId);
            sh.AddShow(CsOrderEnum.OrderAddress);
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
            if (!para.OrderId.IsNullOrEmpty())
                sh.AddWhere(CsOrderEnum.OrderNumber, para.OrderId, RelationEnum.Like);
            if (para.Status > -1)
                sh.AddWhere(CsOrderEnum.OrderState, para.Status);
            if (!para.UserName.IsNullOrEmpty())
            {
                if (para.UserName.ToInt() > 0)
                {
                    sh.AddWhere("cu." + CsUsersEnum.UserId, para.UserName.ToInt());
                }
                else
                {
                    sh.AddWhere("cu." + CsUsersEnum.UserName, para.UserName, RelationEnum.Like);
                }
            }
            if (!para.UserPhone.IsNullOrEmpty())
                sh.AddWhere("cu." + CsUsersEnum.UserPhone, para.UserPhone, RelationEnum.Like);
            if (para.Time.Count > 0)
            {
                if (!para.Time[0].IsNullOrEmpty())
                {
                    sh.AddWhere(CsOrderEnum.OrderDate, para.Time[0], RelationEnum.GreaterEqual);
                }
                if (!para.Time[1].IsNullOrEmpty())
                {
                    sh.AddWhere(CsOrderEnum.OrderDate, para.Time[1], RelationEnum.LessEqual);
                }
            }

            return new PageInfo<CsOrderView.CsOrderPage>
            {
                Data = sh.Select().ToList(),
                Total = sh.Total,
                Sql = sh.SqlString.ToString()
            };
        }

        private ExcelRow ExcelRowType(CsOrderView.CsOrderImport row)
        {
            if (row.实收金额.IsNullOrEmpty() &&
                row.总金额.IsNullOrEmpty() &&
                row.收货人.IsNullOrEmpty() &&
                row.收货地址.IsNullOrEmpty() &&
                row.联系电话.IsNullOrEmpty() &&
                !row.商品名称.IsNullOrEmpty() &&
                !row.种类.IsNullOrEmpty() &&
                !row.单价.IsNullOrEmpty() &&
                !row.数量.IsNullOrEmpty()
            )
            {
                return ExcelRow.Detail;
            }
            if (row.实收金额.IsNullOrEmpty() &&
                row.总金额.IsNullOrEmpty() &&
                row.收货人.IsNullOrEmpty() &&
                row.收货地址.IsNullOrEmpty() &&
                row.联系电话.IsNullOrEmpty() &&
                row.货运单号.IsNullOrEmpty() &&
                row.商品名称.IsNullOrEmpty() &&
                row.种类.IsNullOrEmpty() &&
                row.单价.IsNullOrEmpty() &&
                row.数量.IsNullOrEmpty())
            {
                return ExcelRow.Empty;
            }
            if (!row.实收金额.IsNullOrEmpty() &&
                !row.总金额.IsNullOrEmpty() &&
                !row.收货人.IsNullOrEmpty() &&
                !row.收货地址.IsNullOrEmpty() &&
                !row.联系电话.IsNullOrEmpty())
            {
                return ExcelRow.Order;
            }
            return ExcelRow.Other;
        }
    }
}