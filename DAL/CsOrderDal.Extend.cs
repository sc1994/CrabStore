using System;
using System.Collections.Generic;
using Common;
using Model.ViewModel;
using Model.DBModel;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Linq;

namespace DAL
{
    /// <summary>
    /// 订单表  数据访问扩展层(此类中的代码不会被覆盖)
    /// </summary>
    public partial class CsOrderDal
    {
        public static Random random = new Random();
        /// <summary>
        /// 根据产品编号查询销售总数
        /// </summary>
        /// <param name="productIds">产品编号</param>
        /// <returns></returns>
        public IEnumerable<CsOrderView.CsOrderTotalByProduct> TotalNumber(string productIds)
        {
            var strSql = " select a.ProductId,sum(ProductNumber) as Total from CsOrderDetail a " +
                         " inner join " +
                         " CsOrder b on a.OrderId = b.OrderId " +
                         " where a.ProductId IN(" + (productIds.IsNullOrEmpty() ? "0" : productIds) + ")" +
                         " and Convert(varchar(7),b.OrderDate,23)='" + DateTime.Now.ToString("yyyy-MM") + "' GROUP BY a.ProductId";
            return DbClient.Query<CsOrderView.CsOrderTotalByProduct>(strSql);
        }
        /// <summary>
        /// 根据产品编号与月份查询销售总数
        /// 根据产品编号查询销售总数
        /// </summary>
        /// <param name="productId">产品编号</param>
        /// <param name="nowTime">月份</param>
        /// <returns></returns>
        public int TotalNumber(int productId, DateTime nowTime)
        {
            var strSql = "select sum(ProductNumber*OrderCopies) from CsOrderDetail a inner join CsOrder b on a.OrderId = b.OrderId where a.ProductId= " + productId
                        + " and Convert(varchar(7),b.OrderDate,23)='" + nowTime.ToString("yyyy-MM") + "'";
            int day = DateTime.Now.Day;
            int number = random.Next(100 * day, 200 * (day + 1));
            return DbClient.ExecuteScalar<int>(strSql) + number;
        }

        /// <summary>
        /// 带有事务操作 添加支付订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int AddOrder(OrderModel order, out string orderNumber)
        {
            var number = 0;//实际操作影响行数
            using (var conn = (SqlConnection)DataSource.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        orderNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        //添加订单表 获取订单编号
                        var _csOrder = new CsOrder();
                        _csOrder.OrderNumber = orderNumber;
                        _csOrder.UserId = order.userid;
                        _csOrder.TotalMoney = order.totalmoney;
                        _csOrder.DiscountMoney = 0;
                        _csOrder.ActualMoney = order.finalPrice;
                        _csOrder.OrderDate = DateTime.Now;
                        _csOrder.OrderState = 1;
                        _csOrder.OrderAddress = order.orderaddress;
                        _csOrder.SendAddress = order.sendaddress;
                        _csOrder.CargoNumber = 1;
                        _csOrder.OrderCopies = order.totalnumber;
                        _csOrder.TotalWeight = order.totalweight;
                        _csOrder.BillWeight = order.sendweight;
                        _csOrder.ExpressMoney = order.expressmoney;
                        _csOrder.ServiceMoney = order.servicemoney;
                        _csOrder.RowStatus = 1;
                        _csOrder.IsInvoice = order.isInvoice ? 1 : 0;
                        _csOrder.OrderRemarks = order.remarks;
                        var strSql1 = new StringBuilder();
                        strSql1.Append("insert into CsOrder (OrderNumber,UserId,TotalMoney,DiscountMoney,ActualMoney,OrderDate,OrderState,OrderAddress,");
                        strSql1.Append("SendAddress,CargoNumber,OrderCopies,TotalWeight,BillWeight,RowStatus,ExpressMoney,ServiceMoney,IsInvoice,OrderRemarks ) values (@OrderNumber,");
                        strSql1.Append("@UserId,@TotalMoney,@DiscountMoney,@ActualMoney,@OrderDate,@OrderState,@OrderAddress,@SendAddress,@CargoNumber,@OrderCopies,");
                        strSql1.Append("@TotalWeight,@BillWeight,@RowStatus,@ExpressMoney,@ServiceMoney,@IsInvoice,@OrderRemarks);select @@Identity;");
                        SqlParameter[] parameter1 =
                        {
                            new SqlParameter("@OrderNumber",SqlDbType.VarChar,50),
                            new SqlParameter("@UserId",SqlDbType.Int,4),
                            new SqlParameter("@TotalMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@DiscountMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@ActualMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@OrderDate",SqlDbType.DateTime),
                            new SqlParameter("@OrderState",SqlDbType.Int,4),
                            new SqlParameter("@OrderAddress",SqlDbType.NVarChar,500),
                            new SqlParameter("@SendAddress",SqlDbType.VarChar,5000),
                            new SqlParameter("@CargoNumber",SqlDbType.Int,4),
                            new SqlParameter("@OrderCopies",SqlDbType.Int,4),
                            new SqlParameter("@TotalWeight",SqlDbType.Decimal,18),
                            new SqlParameter("@BillWeight",SqlDbType.Decimal,18),
                            new SqlParameter("@RowStatus",SqlDbType.TinyInt,4),
                            new SqlParameter("@ExpressMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@ServiceMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@IsInvoice",SqlDbType.Int,4),
                            new SqlParameter("@OrderRemarks",SqlDbType.Text)
                        };
                        parameter1[0].Value = _csOrder.OrderNumber;
                        parameter1[1].Value = _csOrder.UserId;
                        parameter1[2].Value = _csOrder.TotalMoney;
                        parameter1[3].Value = _csOrder.DiscountMoney;
                        parameter1[4].Value = _csOrder.ActualMoney;
                        parameter1[5].Value = _csOrder.OrderDate;
                        parameter1[6].Value = _csOrder.OrderState;
                        parameter1[7].Value = _csOrder.OrderAddress;
                        parameter1[8].Value = _csOrder.SendAddress;
                        parameter1[9].Value = _csOrder.CargoNumber;
                        parameter1[10].Value = _csOrder.OrderCopies;
                        parameter1[11].Value = _csOrder.TotalWeight;
                        parameter1[12].Value = _csOrder.BillWeight;
                        parameter1[13].Value = _csOrder.RowStatus;
                        parameter1[14].Value = _csOrder.ExpressMoney;
                        parameter1[15].Value = _csOrder.ServiceMoney;
                        parameter1[16].Value = _csOrder.IsInvoice;
                        parameter1[17].Value = _csOrder.OrderRemarks;
                        object obj = DbClient.ExecuteScalar(conn, trans, strSql1.ToString(), parameter1);
                        if (obj != null)
                        {
                            int orderid = Convert.ToInt32(obj);
                            int totalNumber = 0;//应总共操作记录影响行数
                            //decimal weight = 0;
                            int carbNumber = 0;//总共购买螃蟹数
                            //添加购物详细
                            //购买螃蟹列表
                            List<CsOrderDetail> detailList = new List<CsOrderDetail>();
                            foreach (CartItem cart in order.cartFoodList)
                            {
                                CsOrderDetail orderDetail = new CsOrderDetail();
                                orderDetail.OrderId = orderid;
                                orderDetail.ProductId = cart.id;
                                orderDetail.UnitPrice = cart.price;
                                orderDetail.ProductNumber = cart.num;
                                orderDetail.TotalPrice = (cart.price * cart.weight * 2 * cart.num);
                                orderDetail.ChoseType = 1;
                                //weight += (cart.weight * cart.num);
                                totalNumber++;
                                carbNumber += cart.num;
                                detailList.Add(orderDetail);
                            }

                            //购买可选配件列表
                            if (order.partNumList.Count > 0)
                            {
                                foreach (CartItem cart in order.partNumList)
                                {
                                    if (cart.id == 0)
                                    {
                                        continue;
                                    }
                                    CsOrderDetail orderDetail = new CsOrderDetail();
                                    orderDetail.OrderId = orderid;
                                    orderDetail.ProductId = cart.id;
                                    orderDetail.UnitPrice = cart.price;
                                    orderDetail.ProductNumber = cart.id == 10009 ? carbNumber : 1;
                                    orderDetail.TotalPrice = (cart.price * cart.num);
                                    orderDetail.ChoseType = 2;
                                    detailList.Add(orderDetail);
                                    totalNumber++;
                                }
                            }
                            //购买必选配件列表
                            foreach (PartItem part in order.partList)
                            {
                                CsOrderDetail orderDetail = new CsOrderDetail();
                                orderDetail.OrderId = orderid;
                                orderDetail.ProductId = part.PartId;
                                orderDetail.UnitPrice = part.PartPrice;
                                orderDetail.ProductNumber = part.PartId == 10004 ? carbNumber : 1;
                                orderDetail.TotalPrice = (part.PartPrice * orderDetail.ProductNumber);
                                orderDetail.ChoseType = 2;
                                detailList.Add(orderDetail);
                                totalNumber++;
                            }
                            foreach (CsOrderDetail detail in detailList)
                            {
                                StringBuilder strSql2 = new StringBuilder();
                                strSql2.Append("insert into CsOrderDetail (OrderId,ProductId,UnitPrice,ProductNumber,TotalPrice,ChoseType )");
                                strSql2.Append("values(@OrderId,@ProductId,@UnitPrice,@ProductNumber,@TotalPrice,@ChoseType)");
                                SqlParameter[] parameter2 =
                                {
                                    new SqlParameter("@OrderId",SqlDbType.Int,4),
                                    new SqlParameter("@ProductId",SqlDbType.Int,4),
                                    new SqlParameter("@UnitPrice",SqlDbType.Money,18),
                                    new SqlParameter("@ProductNumber",SqlDbType.Int),
                                    new SqlParameter("@TotalPrice",SqlDbType.Decimal,18),
                                    new SqlParameter("@ChoseType",SqlDbType.Int,4)
                                };
                                parameter2[0].Value = detail.OrderId;
                                parameter2[1].Value = detail.ProductId;
                                parameter2[2].Value = detail.UnitPrice;
                                parameter2[3].Value = detail.ProductNumber;
                                parameter2[4].Value = detail.TotalPrice;
                                parameter2[5].Value = detail.ChoseType;
                                number += DbClient.ExecuteSql(conn, trans, strSql2.ToString(), parameter2);
                            }
                            if (number != totalNumber)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            else
                            {
                                trans.Commit();
                                return orderid;
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex.ToJson(), "订单添加异常");
                        trans.Rollback();
                        orderNumber = "";
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 根据订单编号，修改与支付编号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="prepaymentid"></param>
        /// <returns></returns>
        public int UpdatePrepaymentId(int orderId, string prepaymentid)
        {
            string strSql = $"update CsOrder set PrepaymentId='{prepaymentid}' where OrderId={orderId}";
            return DbClient.Excute(strSql);
        }

        /// <summary>
        /// 根据订单编号，修改订单状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        public int UpdateOrderState(int orderid, int orderState)
        {
            string strSql = $"update CsOrder set OrderState={orderState} where OrderId={orderid}";
            return DbClient.Excute(strSql);
        }
        public string GetPrepaymentId(int orderId)
        {
            string strSql = $"select top 1 PrepaymentId from CsOrder where OrderId={orderId}";
            return DbClient.ExecuteScalar<string>(strSql);
        }

        /// <summary>
        /// 根据openid 得到订单列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public List<CsOrder> GetModelListByOpenId(string openId, int num, int size, out int total)
        {
            var strSql = new StringBuilder();
            strSql.Append($"SELECT top ({size}) * FROM ( SELECT ");
            strSql.Append($"ROW_NUMBER() OVER ( ORDER BY OrderId DESC ) AS ROWNUMBER,b.* ");
            strSql.Append(" FROM  CsOrder b inner join CsUsers c on b.UserId = c.UserId ");
            strSql.Append($" WHERE 1 = 1 and RowStatus=1 and c.OpenId='{openId}' ");
            strSql.Append(" ) A");
            strSql.Append($" WHERE ROWNUMBER BETWEEN {(num - 1) * size + 1} AND {num * size}; ");
            total = DbClient.ExecuteScalar<int>($"SELECT COUNT(1) FROM CsOrder b inner join CsUsers c on b.UserId = c.UserId WHERE 1 = 1 and RowStatus=1 and c.OpenId='{openId}';");
            return DbClient.Query<CsOrder>(strSql.ToString()).ToList();
        }

        /// <summary>
        /// 支付完成，修改订单状态，生成扣除库存与修改用户购买重量
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int FinshOrder(int orderId, int userId, decimal totalWeight, int orderCopies)
        {
            int number = 0;//实际操作影响行数

            try
            {
                //第一步骤：修改订单状态为支付完成但未配货
                string strSql1 = $"update CsOrder set OrderState=2 where OrderId={orderId}";
                number = DbClient.Excute(strSql1);
                //第二步骤:修改螃蟹库存
                CsOrderDetailDal orderDetailDal = new CsOrderDetailDal();
                //查询出本次订单中购买螃蟹列表
                List<CsOrderDetail> DetailList = orderDetailDal.GetModelList(" and OrderId=" + orderId + " and ChoseType=1");
                if (DetailList.Count > 0)
                {
                    foreach (CsOrderDetail OrderDetail in DetailList)
                    {
                        string strSql2 = $"update CsProducts set ProductStock=ProductStock-{OrderDetail.ProductNumber} where ProductId={OrderDetail.ProductId}";
                        DbClient.Excute(strSql2);
                    }
                }
                //第三步，修改用户的购买累计重量
                decimal total = totalWeight * orderCopies;
                string strSql3 = $"update CsUsers set TotalWight=TotalWight+{total} where UserId={userId}";
                DbClient.Excute(strSql3);
                if (number > 0)
                {

                    return number;
                }
                else
                {

                    return 0;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message,"完成支付出错");
                return 0;
            }
        }

        #region 套餐订单
        /// <summary>
        /// 添加套餐订单
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="orderNumber">订单编号</param>
        /// <returns></returns>
        public int AddPackageOrder(OrderModel order, out string orderNumber)
        {
            int number = 0;//实际操作影响行数
            using (SqlConnection conn = (SqlConnection)DataSource.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        orderNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        //添加订单表 获取订单编号
                        CsOrder _csOrder = new CsOrder();
                        _csOrder.OrderNumber = orderNumber;
                        _csOrder.UserId = order.userid;
                        _csOrder.TotalMoney = order.totalmoney;
                        _csOrder.DiscountMoney = 0;
                        _csOrder.ActualMoney = order.finalPrice;
                        _csOrder.OrderDate = DateTime.Now;
                        _csOrder.OrderState = 1;
                        _csOrder.OrderAddress = order.orderaddress;
                        _csOrder.SendAddress = order.sendaddress;
                        _csOrder.CargoNumber = 1;
                        _csOrder.OrderCopies = order.totalnumber;
                        _csOrder.TotalWeight = order.totalweight;
                        _csOrder.BillWeight = order.sendweight;
                        _csOrder.ExpressMoney = order.expressmoney;
                        _csOrder.ServiceMoney = order.servicemoney;
                        _csOrder.RowStatus = 1;
                        _csOrder.IsInvoice = order.isInvoice ? 1 : 0;
                        _csOrder.OrderRemarks = order.remarks;
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.Append("insert into CsOrder (OrderNumber,UserId,TotalMoney,DiscountMoney,ActualMoney,OrderDate,OrderState,OrderAddress,");
                        strSql1.Append("SendAddress,CargoNumber,OrderCopies,TotalWeight,BillWeight,RowStatus,ExpressMoney,ServiceMoney,IsInvoice,OrderRemarks ) values (@OrderNumber,");
                        strSql1.Append("@UserId,@TotalMoney,@DiscountMoney,@ActualMoney,@OrderDate,@OrderState,@OrderAddress,@SendAddress,@CargoNumber,@OrderCopies,");
                        strSql1.Append("@TotalWeight,@BillWeight,@RowStatus,@ExpressMoney,@ServiceMoney,@IsInvoice,@OrderRemarks);select @@Identity;");
                        SqlParameter[] parameter1 =
                        {
                            new SqlParameter("@OrderNumber",SqlDbType.VarChar,50),
                            new SqlParameter("@UserId",SqlDbType.Int,4),
                            new SqlParameter("@TotalMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@DiscountMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@ActualMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@OrderDate",SqlDbType.DateTime),
                            new SqlParameter("@OrderState",SqlDbType.Int,4),
                            new SqlParameter("@OrderAddress",SqlDbType.NVarChar,500),
                            new SqlParameter("@SendAddress",SqlDbType.VarChar,5000),
                            new SqlParameter("@CargoNumber",SqlDbType.Int,4),
                            new SqlParameter("@OrderCopies",SqlDbType.Int,4),
                            new SqlParameter("@TotalWeight",SqlDbType.Decimal,18),
                            new SqlParameter("@BillWeight",SqlDbType.Decimal,18),
                            new SqlParameter("@RowStatus",SqlDbType.TinyInt,4),
                            new SqlParameter("@ExpressMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@ServiceMoney",SqlDbType.Decimal,18),
                            new SqlParameter("@IsInvoice",SqlDbType.Int,4),
                            new SqlParameter("@OrderRemarks",SqlDbType.Text),
                        };
                        parameter1[0].Value = _csOrder.OrderNumber;
                        parameter1[1].Value = _csOrder.UserId;
                        parameter1[2].Value = _csOrder.TotalMoney;
                        parameter1[3].Value = _csOrder.DiscountMoney;
                        parameter1[4].Value = _csOrder.ActualMoney;
                        parameter1[5].Value = _csOrder.OrderDate;
                        parameter1[6].Value = _csOrder.OrderState;
                        parameter1[7].Value = _csOrder.OrderAddress;
                        parameter1[8].Value = _csOrder.SendAddress;
                        parameter1[9].Value = _csOrder.CargoNumber;
                        parameter1[10].Value = _csOrder.OrderCopies;
                        parameter1[11].Value = _csOrder.TotalWeight;
                        parameter1[12].Value = _csOrder.BillWeight;
                        parameter1[13].Value = _csOrder.RowStatus;
                        parameter1[14].Value = _csOrder.ExpressMoney;
                        parameter1[15].Value = _csOrder.ServiceMoney;
                        parameter1[16].Value = _csOrder.IsInvoice;
                        parameter1[17].Value = _csOrder.OrderRemarks;
                        object obj = DbClient.ExecuteScalar(conn, trans, strSql1.ToString(), parameter1);
                        if (obj != null)
                        {
                            int orderid = Convert.ToInt32(obj);
                            int totalNumber = 0;//应总共操作记录影响行数

                            //添加购物详细
                            //购买套餐列表
                            List<CsOrderDetail> detailList = new List<CsOrderDetail>();
                            if (order.cartFoodList.Count > 0)
                            {
                                foreach (CartItem cart in order.cartFoodList)
                                {
                                    if (cart.id == 0)
                                    {
                                        continue;
                                    }
                                    CsOrderDetail orderDetail = new CsOrderDetail();
                                    orderDetail.OrderId = orderid;
                                    orderDetail.ProductId = cart.id;
                                    orderDetail.UnitPrice = cart.price;
                                    orderDetail.ProductNumber = cart.num;
                                    orderDetail.TotalPrice = cart.price * cart.num;
                                    orderDetail.ChoseType = 3;
                                    totalNumber++;
                                    detailList.Add(orderDetail);
                                }
                            }
                            foreach (CsOrderDetail detail in detailList)
                            {
                                StringBuilder strSql2 = new StringBuilder();
                                strSql2.Append("insert into CsOrderDetail (OrderId,ProductId,UnitPrice,ProductNumber,TotalPrice,ChoseType )");
                                strSql2.Append("values(@OrderId,@ProductId,@UnitPrice,@ProductNumber,@TotalPrice,@ChoseType)");
                                SqlParameter[] parameter2 =
                                {
                                    new SqlParameter("@OrderId",SqlDbType.Int,4),
                                    new SqlParameter("@ProductId",SqlDbType.Int,4),
                                    new SqlParameter("@UnitPrice",SqlDbType.Money,18),
                                    new SqlParameter("@ProductNumber",SqlDbType.Int),
                                    new SqlParameter("@TotalPrice",SqlDbType.Decimal,18),
                                    new SqlParameter("@ChoseType",SqlDbType.Int,4)
                                };
                                parameter2[0].Value = detail.OrderId;
                                parameter2[1].Value = detail.ProductId;
                                parameter2[2].Value = detail.UnitPrice;
                                parameter2[3].Value = detail.ProductNumber;
                                parameter2[4].Value = detail.TotalPrice;
                                parameter2[5].Value = detail.ChoseType;
                                number += DbClient.ExecuteSql(conn, trans, strSql2.ToString(), parameter2);
                            }
                            if (number != totalNumber)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            else
                            {
                                trans.Commit();
                                return orderid;
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex.Message);
                        trans.Rollback();
                        orderNumber = "";
                        return 0;
                    }
                }

            }
        }

        /// <summary>
        /// 根据订单编号，完成订单支付后续操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int FinshPackageOrder(int orderId)
        {
            int number = 0;//实际操作影响行数
            using (SqlConnection conn = (SqlConnection)DataSource.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //第一步骤：修改订单状态为支付完成但未配货
                        string strSql1 = $"update CsOrder set OrderState=2 where OrderId={orderId}";
                        number += DbClient.ExecuteSql(conn, trans, strSql1, null);
                        //第二步骤:修改套餐库存
                        CsOrderDetailDal orderDetailDal = new CsOrderDetailDal();
                        //查询出本次订单中购买螃蟹列表
                        List<CsOrderDetail> DetailList = orderDetailDal.GetModelList(" and OrderId=" + orderId + " and ChoseType=3");
                        if (DetailList.Count > 0)
                        {
                            foreach (CsOrderDetail OrderDetail in DetailList)
                            {
                                string strSql2 = $"update CsPackage set PackageStock=PackageStock-{OrderDetail.ProductNumber} where PackageId={OrderDetail.ProductId}";
                                number += DbClient.ExecuteSql(conn, trans, strSql2, null);
                            }
                        }
                        //第三步，修改用户的购买累计重量
                        //decimal total = totalWeight * orderCopies;
                        //string strSql3 = $"update CsUsers set TotalWight=TotalWight+{total} where UserId={userId}";
                        //number += DbClient.ExecuteSql(conn, trans, strSql3, null);
                        if (number == (DetailList.Count + 1))
                        {
                            trans.Commit();
                            return number;
                        }
                        else
                        {
                            trans.Rollback();
                            return 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex.Message);
                        trans.Rollback();
                        return 0;
                    }
                }
            }
        }
        #endregion

    }
}
