using System;
using System.Collections;
using System.Collections.Generic;
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

            var list = sh.Select().Select(x => new
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
                sql = sh.SqlString.ToString(),
                total = sh.Total
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
    }
}