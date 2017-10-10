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
        private readonly CsPackageBll _csPackageBll = new CsPackageBll();

        // GET: CsOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsOrderPage(CsOrderView.CsOrderWhere para)
        {
            var data = GetList(para);

            if (!data.Data.Any())
            {
                return Json(new
                {
                    data = data.Data,
                    sql = data.Sql,
                    total = data.Total
                });
            }

            var ds = _csOrderDetailBll.GetModelList($" AND OrderId IN ({string.Join(",", data.Data.Select(x => x.OrderId))}) ");

            var list = data.Data.Select(x => new
            {
                x.OrderId,
                x.OrderNumber,
                OrderDate = x.OrderDate.ToString("yyyy-M-d HH:mm:ss"),
                OrderState = ((OrderState)x.OrderState).ToString(),
                RowStatus = ((RowStatus)x.RowStatus).ToString(),
                DeleteDate = x.DeleteDate.ToString("yyyy-M-d HH:mm:ss"),
                x.DeleteDescribe,
                UserName = x.UserId + " / " + x.UserName + $"({x.UserSex}) / " + x.UserPhone,
                ShortUserName = (x.UserId + " / " + x.UserName + $"({x.UserSex}) / " + x.UserPhone).SubString(24),
                OrderAddress = x.OrderAddress.Trim('$').Replace("$$$", " / ").Replace("$$", " / ").Replace("$", " / "),
                ShortOrderAddress = x.OrderAddress.Trim('$').Replace("$$$", " / ").Replace("$$", " / ").Replace("$", " / ").SubString(30),
                TotalMoney = "￥" + x.TotalMoney.ToString("N2"),
                OrderSource = ds.Any(d => d.OrderId == x.OrderId && d.ChoseType == ChoseType.套餐.GetHashCode()) ? "企业团购" : "电商代发",
                IsInvoice = x.IsInvoice == 0 ? "否" : "是",
                ShortOrderRemarks = x.OrderRemarks.SubString(16).ShowNullOrEmpty("-无-"),
                OrderRemarks = x.OrderRemarks.ShowNullOrEmpty("-无-")
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
            var packages = new List<CsPackage>();

            #region 相关 套餐/配件和蟹
            if (csOrderDetails.Any(x => x.ChoseType == ChoseType.配件.GetHashCode()))
            {
                parts = _csPartsBll.GetModelList($" AND PartId IN ({string.Join(",", csOrderDetails.Where(x => x.ChoseType == ChoseType.配件.GetHashCode()).Select(x => x.ProductId))})");
            }
            if (csOrderDetails.Any(x => x.ChoseType == ChoseType.螃蟹.GetHashCode()))
            {
                products = _csProductsBll.GetModelList($" AND ProductId IN ({string.Join(",", csOrderDetails.Where(x => x.ChoseType == ChoseType.螃蟹.GetHashCode()).Select(x => x.ProductId))})");
            }
            if (csOrderDetails.Any(x => x.ChoseType == ChoseType.套餐.GetHashCode()))
            {
                packages = _csPackageBll.GetModelList($" AND PackageId IN ({string.Join(",", csOrderDetails.Where(x => x.ChoseType == ChoseType.套餐.GetHashCode()).Select(x => x.ProductId))})");
            }

            #endregion

            #region 订单详细列表
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
                    var part = parts.FirstOrDefault(x => x.PartId == csOrderDetailExtend.ProductId);
                    csOrderDetailExtend.ProductName = $"{(part?.PartName).ShowNullOrEmpty()}({(part?.PartNumber).ShowNullOrEmpty()})";
                    csOrderDetailExtend.ChoseType = ((PartType?)part?.PartType).ShowNullOrEmpty();
                }
                else if (csOrderDetailExtend.ChoseType == ChoseType.螃蟹.ToString())
                {
                    var product = products.FirstOrDefault(x => x.ProductId == csOrderDetailExtend.ProductId);
                    csOrderDetailExtend.ProductName = $"{(product?.ProductName).ShowNullOrEmpty()}({(product?.ProductNumber).ShowNullOrEmpty()})";
                    csOrderDetailExtend.ChoseType = ((ProductType?)product?.ProductType).ShowNullOrEmpty();
                }
                else
                {
                    var package = packages.FirstOrDefault(x => x.PackageId == csOrderDetailExtend.ProductId);
                    csOrderDetailExtend.ProductName = $"{(package?.PackageName).ShowNullOrEmpty()}({(package?.PackageNumber).ShowNullOrEmpty()})";
                }
                csOrderDetailExtends.Add(csOrderDetailExtend);
            }
            #endregion

            var sendInfo = csOrder.SendAddress.Split('$');
            var putInfo = csOrder.OrderAddress.Split('$');
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
                OrderCopies = csOrder.OrderCopies + " 份",
                TotalWeight = csOrder.TotalWeight + " KG",
                BillWeight = csOrder.BillWeight + " KG",
                UserName = $"{user.UserName}({user.UserSex})",
                UserPhone = user.UserPhone,
                CsOrderDetails = csOrderDetailExtends,
                OrderDelivery = csOrder.OrderDelivery,
                OrderAddress = csOrder.OrderAddress.Trim('$').Replace("$$", "$").Replace("$", "//"),
                OrderConsignee = putInfo.Length > 1 ? putInfo[1] : "-",
                OrderTelPhone = putInfo.Length > 3 ? putInfo[3] : "-",
                OrderDetails = putInfo.Length > 4 ? putInfo[4] : "-",
                SendConsignee = sendInfo.Length > 0 ? sendInfo[0] : "-",
                SendTelPhone = sendInfo.Length > 1 ? sendInfo[1] : "",
                SendAddress = csOrder.SendAddress.Trim('$').Replace("$$", "$").Replace("$", "//"),
                OrderSource = csOrderDetails.Any(x => x.ChoseType == ChoseType.套餐.GetHashCode()) ? "企业团购" : "电商代发",
                PrepaymentId = csOrder.PrepaymentId.IsNullOrEmpty() ? "未生成预支付编号" : csOrder.PrepaymentId,
                IsInvoice = csOrder.IsInvoice.ToString(),
                OrderRemarks = csOrder.OrderRemarks
            });
        }

        public ActionResult UpdateCsOrder(CsOrderView.CsOrderUpdate para)
        {
            if (para.id < 1)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "缺少必要的Id"
                });
            }

            var model = _csOrderBll.GetModel(para.id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "Id 未能查询到对应订单, 请刷新页面再试"
                });
            }
            model.RowStatus = para.rowStatus;
            model.IsInvoice = para.isInvoice;
            model.OrderRemarks = para.orderRemarks;
            if (para.rowStatus == RowStatus.无效.GetHashCode())
            {
                model.DeleteDate = para.deleteDate;
                model.DeleteDescribe = para.deleteDescribe;
            }
            else
            {
                model.DeleteDate = "1900-1-1".ToDate();
                model.DeleteDescribe = "";
            }
            model.OrderState = para.orderState;
            if (para.orderState == OrderState.已发货.GetHashCode())
            {
                model.OrderDelivery = para.delivery;
            }
            else
            {
                model.OrderDelivery = "";
            }
            model.SendAddress = para.sendConsignee + "$" + para.sendTelphone;
            model.OrderAddress = "$" + para.orderConsignee + "$$" + para.orderTelphone + "$" + para.orderDetails;
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
            if (!data.Data.Any())
            {
                return Json(new ResModel
                {
                    Data = "没有任何可以导出数据",
                    ResStatus = ResStatue.No
                });
            }
            var list = new List<CsOrderView.CsOrderExcel>();
            var detail = _csOrderDetailBll.GetModelList($" AND OrderId IN ({string.Join(",", data.Data.Select(x => x.OrderId))})");

            // 配件/螃蟹/套餐
            var parts = _csPartsBll.GetModelList($" AND PartId IN ({string.Join(",", detail.Where(x => x.ChoseType == ChoseType.配件.GetHashCode()).Select(x => x.ProductId)).ShowNullOrEmpty("0")}) AND PartType = {PartType.可选配件.GetHashCode()}");
            var products = _csProductsBll.GetModelList($" AND ProductId IN ({string.Join(",", detail.Where(x => x.ChoseType == ChoseType.螃蟹.GetHashCode()).Select(x => x.ProductId)).ShowNullOrEmpty("0")})");
            IEnumerable<CsPackage> packages = null;
            if (detail.Any(x => x.ChoseType == ChoseType.套餐.GetHashCode()))
            {
                packages = _csPackageBll.GetModelList($" AND PackageId IN ({string.Join(",", detail.Where(x => x.ChoseType == ChoseType.套餐.GetHashCode()).Select(x => x.ProductId))})");
            }
            foreach (var order in data.Data)
            {
                var total = string.Empty;
                foreach (var d in detail.Where(x => x.OrderId == order.OrderId).OrderBy(x => x.ChoseType))
                {
                    if (d.ChoseType == ChoseType.套餐.GetHashCode())
                    {
                        total += packages?.FirstOrDefault(x => x.PackageId == d.ProductId)?.PackageNumber + "" + d.ProductNumber;
                        continue;
                    }
                    if (d.ChoseType == ChoseType.螃蟹.GetHashCode())
                    {
                        total += products.FirstOrDefault(x => x.ProductId == d.ProductId)?.ProductNumber + "" + d.ProductNumber;
                        continue;
                    }
                    if (total.IndexOf("P", StringComparison.Ordinal) < 0 &&
                        total.IndexOf("-", StringComparison.Ordinal) < 0)
                    {
                        total += "-";
                    }
                    total += parts.FirstOrDefault(x => x.PartId == d.ProductId)?.PartNumber;
                }
                var sendInfo = order.SendAddress.Split('$');
                var putInfo = order.OrderAddress.Split('$');
                list.Add(new CsOrderView.CsOrderExcel
                {
                    用户订单号 = order.OrderNumber,
                    寄件公司 = "-",
                    寄联系人 = sendInfo.Length > 0 ? sendInfo[0] : "-",
                    寄联系电话 = sendInfo.Length > 1 ? sendInfo[1] : "-",
                    寄件地址 = sendInfo.Length > 2 ? sendInfo[2] : "-",
                    收件公司 = putInfo.Length > 0 ? putInfo[0] : "-",
                    联系人 = putInfo.Length > 1 ? putInfo[1] : "-",
                    联系电话 = putInfo.Length > 2 ? putInfo[2] : "-",
                    手机号码 = putInfo.Length > 3 ? putInfo[3] : "-",
                    收件详细地址 = putInfo.Length > 4 ? putInfo[4] : "-",
                    付款方式 = "寄付月结",
                    第三方付月结卡号 = "",
                    寄托物品 = "其他",
                    寄托物内容 = total.TrimEnd('-').IsNullOrEmpty() ? "" : $"{total.TrimEnd('-')}-T{detail.Where(x => x.OrderId == order.OrderId && (x.ChoseType == ChoseType.螃蟹.GetHashCode() || x.ChoseType == ChoseType.套餐.GetHashCode())).Sum(x => x.ProductNumber)}",
                    寄托物编号 = "",
                    寄托物数量 = order.CargoNumber.ToString(),
                    件数 = order.OrderCopies.ToString(),
                    实际重量单位KG = "-",
                    计费重量单位KG = "-",
                    业务类型 = "大闸蟹专递",
                    扩展字段1 = detail.Any(x => x.OrderId == order.OrderId && x.ChoseType == ChoseType.套餐.GetHashCode()) ? "企业团购" : "电商代发",
                    扩展字段2 = order.OrderRemarks
                });
            }
            var path = $"excel/{DateTime.Now:yyyyMMddHHmmssffff}.xls";
            try
            {
                NpoiHelper.ExportToExcel(list.ToDataTable(), "D:/" + path);
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
            IList<CsOrderView.CsOrderImport> orders;
            try
            {
                orders = NpoiHelper.ReadExcel(path.Replace("/" + fileName, ""), fileName).ToList<CsOrderView.CsOrderImport>();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.ToJson(), "execl文件读取失败");
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "execl 文件读取失败,请确认文件数据正确, 请检查日志描述"
                });
            }
            if (!orders.Any())
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "当前Excel中没有数据,请确认文件是否包含规定完整的数据"
                });
            }
            var users = _csUsersBll.GetModelList($" AND (UserName IN ('{string.Join("','", orders.Select(x => x.收货人))}') OR UserPhone IN ('{string.Join("','", orders.Select(x => x.收货人电话))}'))");
            var products = _csProductsBll.GetModelList("");
            var parts = _csPartsBll.GetModelList("");
            var data = new List<CsOrderView.CsOrderAndDetail>();
            var item = new CsOrderView.CsOrderAndDetail();
            var lastType = ExcelRow.Other; // 记录上一行状态
            var endEmpty = new List<int>(); // 记录尾部空行
            var overEndEmpty = false; // 尾部空行是否结束
            for (var i = orders.Count - 1; i >= 0; i--)
            {
                var type = ExcelRowType(orders[i]);
                if (type == ExcelRow.Empty &&
                    !overEndEmpty)
                {
                    endEmpty.Add(i);
                }
                else
                {
                    overEndEmpty = true;
                }
                if (type == ExcelRow.Empty &&
                    lastType == ExcelRow.Empty &&
                    overEndEmpty)
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.No,
                        Data = "当前Excel中,两个订单信息中间,存在连续多空行.请仔细确认数据, 数据一旦上传将无法撤回"
                    });
                }
                lastType = type;
            }

            // 将尾部空行删除只保留一个
            endEmpty.ForEach(x => orders.RemoveAt(x));
            orders.Add(new CsOrderView.CsOrderImport());

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
                var product = products.FirstOrDefault(x => x.ProductNumber == order.商品编码);
                var part = parts.FirstOrDefault(x => x.PartNumber == order.商品编码);
                var pId = 0; // 商品Id
                var cType = ChoseType.螃蟹; // 蟹或配件
                if (product == null && part == null)
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.No,
                        Data = $"商品编码:{order.商品编码},不存在与数据库中,请仔细确认数据"
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
                    var user = users.FirstOrDefault(x => x.UserPhone == order.收货人电话 && x.UserName == order.收货人);
                    int userId;
                    if (user == null)
                    {
                        user = new CsUsers
                        {
                            UserSex = "先生",
                            UserName = order.收货人,
                            UserPhone = order.收货人电话,
                            OpenId = "",
                            Remarks = "",
                            TotalWight = 0,
                            UserBalance = 0,
                            UserState = 1
                        };
                        userId = _csUsersBll.Add(user);
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
                        SendAddress = $"{order.发货人}${order.发货人电话}",
                        OrderAddress = $"${order.收货人}" + $"({user.UserSex})$${order.收货人电话}${order.收货地址})",
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
                if (d.CsOrder.RowStatus == RowStatus.无效.GetHashCode())
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.No,
                        Data = "存在无效订单,请检查表格总的数据是否满足要求"
                    });
                }
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
        /// 批量更新配货中
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult BatchDising(List<int> ids)
        {
            if (!ids.Any())
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "没有查询到需要更新的数据"
                });
            }
            var sh = new SqlHelper<CsOrder>();
            sh.AddUpdate(CsOrderEnum.OrderState.ToString(), OrderState.配货中.GetHashCode());
            sh.AddWhere($" AND OrderId IN ({string.Join(",", ids)})");
            var count = sh.Update();
            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = $"执行成功,更新{count}条数据."
            });

        }

        /// <summary>
        /// 批量更新已发货
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult BatchDis(string path)
        {
            path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path.Replace("..", "");
            var fileArr = path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = fileArr[fileArr.Length - 1];
            IEnumerable<CsOrderView.CsOrderBatchDis> orders;
            try
            {
                orders = NpoiHelper.ReadExcel(path.Replace("/" + fileName, ""), fileName)
                                       .ToList<CsOrderView.CsOrderBatchDis>()
                                       .Where(x => !x.订单编号.IsNullOrEmpty() && !x.运单号.IsNullOrEmpty());
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.ToJson(), "execl文件读取失败");
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "execl 文件读取失败,请确认文件数据正确, 请检查日志描述"
                });
            }

            if (!orders.Any())
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "当前Excel中没有数据,请确认文件是否包含规定完整的数据"
                });
            }
            if (orders.Any(x => x.订单编号.IsNullOrEmpty() || x.运单号.IsNullOrEmpty()))
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "当前Excel中存在空数据,请确认数据的完整性, 数据一旦上传将无法撤回"
                });
            }
            var distinct = orders.Distinct(x => x.订单编号);
            if (distinct.Count() != orders.Count())
            {
                return Json(new
                {
                    code = ResStatue.Warn.GetHashCode(),
                    data = "存在重复的订单编号,请依据给出的数据检查, 数据一旦上传将无法撤回",
                    orderIds = orders.Except(distinct).Select(x => x.订单编号)
                });
            }
            var data = _csOrderBll.GetModelList($" AND OrderNumber IN ('{string.Join("','", orders.Select(x => x.订单编号))}')");
            if (data.Count != orders.Count())
            {
                return Json(new
                {
                    code = ResStatue.Warn.GetHashCode(),
                    data = "存在有误的订单编号,请依据给出的数据检查, 数据一旦上传将无法撤回",
                    orderIds = orders.Select(x => x.订单编号.Trim()).Except(data.Select(x => x.OrderNumber))
                });
            }
            var count = 0;
            foreach (var order in orders)
            {
                var sh = new SqlHelper<CsOrder>();
                sh.AddUpdate("OrderState", OrderState.已发货.GetHashCode());
                sh.AddUpdate("OrderDelivery", order.运单号);
                sh.AddWhere($" AND OrderNumber = '{order.订单编号}'");
                count += sh.Update();
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = $"更新成功, 共计更新{count}条数据."
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
            var shParts = new SqlHelper<CsParts>();

            if (!para.ProductName.IsNullOrEmpty())
            {
                shProducts.AddWhere(CsProductsEnum.ProductName, para.ProductName, RelationEnum.Like);
                shParts.AddWhere(CsPartsEnum.PartName, para.ProductName, RelationEnum.Like);
            }
            if (para.ProductType != 0)
                shProducts.AddWhere(CsProductsEnum.ProductType, para.ProductType);
            if (para.PartType != 0)
                shParts.AddWhere(CsPartsEnum.PartType, para.PartType);

            var products = shProducts.Select();
            var parts = shParts.Select();

            var coms = products.Select(x => x.ProductId).Union(parts.Select(x => x.PartId));

            if (!coms.Any())
            {
                return Json(new ArrayList());
            }
            #endregion

            #region 获取未发货订单数量
            var sh = new SqlHelper<StatisticView.StatisticList>("CsOrderDetail")
            {
                Alia = "cod"
            };
            sh.AddJoin(JoinEnum.LeftJoin, "CsOrder", "co", "OrderId", "OrderId", $" AND ProductId IN ({string.Join(",", coms)})");
            sh.AddShow("OrderState,ProductId,co.OrderId,OrderDate,(ProductNumber * co.OrderCopies) as ProductNumber,ChoseType,co.OrderCopies");
            sh.AddWhere($" AND OrderState IN ({OrderState.支付成功.GetHashCode()},{OrderState.配货中.GetHashCode()})");
            sh.AddWhere(CsOrderEnum.RowStatus, RowStatus.有效.GetHashCode());
            var orders = sh.Select();
            #endregion

            #region 整合数据
            var data = new List<StatisticView.StatisticList>();
            foreach (var com in coms)
            {
                var total = orders
                    .Where(x => x.ProductId == com)
                    .Sum(x => x.ProductNumber);
                if (total <= 0)
                {
                    continue;
                }

                var product = products.FirstOrDefault(x => x.ProductId == com);
                var part = parts.FirstOrDefault(x => x.PartId == com);

                var stock = orders
                    .Where(x => x.ProductId == com && x.OrderState == OrderState.配货中.GetHashCode())
                    .Sum(x => x.ProductNumber);
                var unStork = orders
                    .Where(x => x.ProductId == com && x.OrderState == OrderState.支付成功.GetHashCode())
                    .Sum(x => x.ProductNumber);

                data.Add(com < 10000
                             ? new StatisticView.StatisticList
                             {
                                 ProductId = product?.ProductId,
                                 ProductName = product?.ProductName.ShowNullOrEmpty(),
                                 ProductType = ((ProductType?)product?.ProductType).ShowNullOrEmpty("未知类型"),
                                 Total = $"{total} 只 / 计 {total * product?.ProductWeight} 斤",
                                 Stock = $"{stock} 只 / 计 {stock * product?.ProductWeight} 斤",
                                 UnStork = $"{unStork} 只 / 计 {unStork * product?.ProductWeight} 斤",
                                 UnStorkInt = unStork
                             }
                             : new StatisticView.StatisticList
                             {
                                 ProductId = part?.PartId,
                                 ProductName = part?.PartName.ShowNullOrEmpty(),
                                 ProductType = ((PartType?)part?.PartType).ShowNullOrEmpty("未知类型"),
                                 Total = $"{total} 只 / 计 {total * part?.PartWeight} 斤",
                                 Stock = $"{stock} 只 / 计 {stock * part?.PartWeight} 斤",
                                 UnStork = $"{unStork} 只 / 计 {unStork * part?.PartWeight} 斤",
                                 UnStorkInt = unStork
                             });
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
                    PageSortField = "co." + CsOrderEnum.OrderId,
                    SortEnum = SortEnum.Desc
                },
                Alia = "co"
            };
            sh.AddShow("co." + CsOrderEnum.OrderId);
            sh.AddShow(CsOrderEnum.OrderAddress);
            sh.AddShow(CsOrderEnum.OrderNumber);
            sh.AddShow("co." + CsOrderEnum.UserId);
            sh.AddShow(CsOrderEnum.TotalMoney);
            sh.AddShow(CsOrderEnum.IsInvoice);
            sh.AddShow(CsOrderEnum.OrderRemarks);
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
            sh.AddShow(CsOrderEnum.SendAddress);
            sh.AddShow(CsOrderEnum.BillWeight);
            sh.AddShow(CsOrderEnum.TotalWeight);
            sh.AddShow(CsOrderEnum.CargoNumber);
            sh.AddShow(CsOrderEnum.OrderCopies);

            sh.AddJoin(JoinEnum.LeftJoin, "CsUsers", "cu", "UserId", "UserId");

            if (para.RowStatus > -1)
                sh.AddWhere(CsOrderEnum.RowStatus, para.RowStatus);
            //if (para.ActualStart > 0)
            //    sh.AddWhere(CsOrderEnum.ActualMoney, para.ActualStart, RelationEnum.GreaterEqual);
            //if (para.ActualEnd > 0)
            //    sh.AddWhere(CsOrderEnum.ActualMoney, para.ActualEnd, RelationEnum.LessEqual);
            //if (para.DiscountStart > 0)
            //    sh.AddWhere(CsOrderEnum.DiscountMoney, para.DiscountStart, RelationEnum.GreaterEqual);
            //if (para.DiscountEnd > 0)
            //    sh.AddWhere(CsOrderEnum.DiscountMoney, para.DiscountEnd, RelationEnum.LessEqual);
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

            //#region 订单来源条件
            //if (!para.OrderSource.IsNullOrEmpty() && para.OrderSource.ToInt() == ChoseType.套餐.GetHashCode()) // 电商代发
            //{
            //    sh.AddJoin(JoinEnum.LeftJoin, "CsOrderDetail", "cod", "OrderId", "OrderId", $" AND cod.ChoseType = {ChoseType.套餐.GetHashCode()} ");
            //}
            //#endregion

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
            if (!para.IsInvoice.IsNullOrEmpty())
                sh.AddWhere(CsOrderEnum.IsInvoice, para.IsInvoice.ToInt());

            if (!para.OrderAddress.IsNullOrEmpty())
                sh.AddWhere(CsOrderEnum.OrderAddress, para.OrderAddress, RelationEnum.Like);

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
                row.收货人电话.IsNullOrEmpty() &&
                row.发货人.IsNullOrEmpty() &&
                row.发货人电话.IsNullOrEmpty() &&
                !row.商品编码.IsNullOrEmpty() &&
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
                row.收货人电话.IsNullOrEmpty() &&
                row.发货人.IsNullOrEmpty() &&
                row.发货人电话.IsNullOrEmpty() &&
                row.货运单号.IsNullOrEmpty() &&
                row.商品编码.IsNullOrEmpty() &&
                row.单价.IsNullOrEmpty() &&
                row.数量.IsNullOrEmpty())
            {
                return ExcelRow.Empty;
            }
            if (!row.实收金额.IsNullOrEmpty() &&
                !row.总金额.IsNullOrEmpty() &&
                !row.收货人.IsNullOrEmpty() &&
                !row.收货地址.IsNullOrEmpty() &&
                !row.收货人电话.IsNullOrEmpty() &&
                !row.发货人.IsNullOrEmpty() &&
                !row.发货人电话.IsNullOrEmpty() &&
                !row.商品编码.IsNullOrEmpty() &&
                !row.单价.IsNullOrEmpty() &&
                !row.数量.IsNullOrEmpty())
            {
                return ExcelRow.Order;
            }
            return ExcelRow.Other;
        }


        /*
            清洗收货电话被搞没的情况 
         */
        //public ActionResult RinseData()
        //{
        //    var sh = new SqlHelper<CsOrder>();
        //    var orders = sh.Select();
        //    var ids = "";
        //    foreach (var order in orders)
        //    {
        //        var putInfo = order.OrderAddress.Split('$');
        //        var phone = string.Empty;
        //        var sex = string.Empty;
        //        var address = string.Empty;
        //        var name = string.Empty;

        //        try
        //        {
        //            phone = putInfo[3];
        //            name = putInfo[1].Split('(')[0];
        //            sex = putInfo[1].Split('(')[1].TrimEnd(')');
        //            address = putInfo[4];
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }

        //        if (!phone.IsNullOrEmpty())
        //        {
        //            continue;
        //        }
        //        var shUpdate = new SqlHelper<CsAddress>();
        //        shUpdate.AddWhere("Consignee", name);
        //        shUpdate.AddWhere("ConSex", sex);
        //        shUpdate.AddWhere("Details", address.Replace(" ", "&"));
        //        //江苏&淮安&清河&长征路19号
        //        var newPhone = shUpdate.Select().FirstOrDefault()?.TelPhone;
        //        var shOrder = new SqlHelper<CsOrder>();
        //        shOrder.AddWhere("OrderId", order.OrderId);
        //        shOrder.AddUpdate("OrderAddress", $"${name}({sex})$${newPhone}${address}");
        //        ids += shOrder.Update() + "/" + order.OrderId + ",";
        //    }
        //    return Content(ids);
        //}
    }
}