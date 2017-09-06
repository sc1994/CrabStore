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
            var strSql = "select sum(ProductNumber) from CsOrderDetail a inner join CsOrder b on a.OrderId = b.OrderId where a.ProductId= " + productId
                        + " and Convert(varchar(7),b.OrderDate,23)='" + nowTime.ToString("yyyy-MM") + "'";
            return DbClient.ExecuteScalar<int>(strSql);
        }

        /// <summary>
        /// 带有事务操作 添加支付订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int AddOrder(OrderModel order)
        {
            int number = 0;//实际操作影响行数
            using (SqlConnection conn = (SqlConnection)DataSource.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //添加订单表 获取订单编号
                        CsOrder _csOrder = new CsOrder();
                        _csOrder.OrderNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        _csOrder.UserId = order.userid;
                        _csOrder.TotalMoney = order.totalmoney;
                        _csOrder.DiscountMoney = order.usebalance;
                        _csOrder.ActualMoney = order.finalPrice;
                        _csOrder.OrderDate = DateTime.Now;
                        _csOrder.OrderState = 1;
                        _csOrder.OrderAddress = order.orderaddress;
                        _csOrder.SendAddress = order.sendaddress;
                        _csOrder.CargoNumber = 1;
                        _csOrder.OrderCopies = order.totalNumber;
                        _csOrder.TotalWeight = order.totalweight;
                        _csOrder.BillWeight = order.sendweight;
                        _csOrder.ExpressMoney = order.expressmoney;
                        _csOrder.ServiceMoney = order.servicemoney;
                        _csOrder.RowStatus = 1;
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.Append("insert into CsOrder (OrderNumber,UserId,TotalMoney,DiscountMoney,ActualMoney,OrderDate,OrderState,OrderAddress,");
                        strSql1.Append("SendAddress,CargoNumber,OrderCopies,TotalWeight,BillWeight,RowStatus,ExpressMoney,ServiceMoney ) values (@OrderNumber,");
                        strSql1.Append("@UserId,@TotalMoney,@DiscountMoney,@ActualMoney,@OrderDate,@OrderState,@OrderAddress,@SendAddress,@CargoNumber,@OrderCopies,");
                        strSql1.Append("@TotalWeight,@BillWeight,@RowStatus,@ExpressMoney,@ServiceMoney);select @@Identity;");
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
                            new SqlParameter("@SeviceMoney",SqlDbType.Decimal,18)
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
                        object obj = DbClient.ExecuteScalar(conn, trans, strSql1.ToString(), parameter1);
                        if (obj != null)
                        {
                            int orderid = Convert.ToInt32(obj);
                            int totalNumber = 2;//应总共操作记录影响行数 基础数据2为用户修改与返利记录添加
                            decimal weight = 0;
                            //添加购物详细
                            List<CsOrderDetail> detailList = new List<CsOrderDetail>();
                            foreach (CartItem cart in order.cartFoodList)
                            {
                                CsOrderDetail orderDetail = new CsOrderDetail();
                                orderDetail.OrderId = orderid;
                                orderDetail.ProductId = cart.id;
                                orderDetail.UnitPrice = cart.price;
                                orderDetail.ProductNumber = cart.num;
                                orderDetail.TotalPrice = (cart.price * cart.num);
                                orderDetail.ChoseType = 1;
                                weight += (cart.weight * cart.num);
                                totalNumber += 2;
                                detailList.Add(orderDetail);
                                //修改库存数
                                StringBuilder strSql4 = new StringBuilder();
                                strSql4.Append("update CsProducts set ProductStock=ProductStock-@Number where ProductId=@ProductId ");
                                SqlParameter[] parameter4 =
                                {
                                    new SqlParameter("@Number",SqlDbType.Int,4),
                                    new SqlParameter("@ProductId",SqlDbType.Int,4)
                                };
                                parameter4[0].Value = cart.num * order.totalNumber;
                                parameter4[1].Value = cart.id;
                                number += DbClient.ExecuteSql(conn, trans, strSql4.ToString(), parameter4);
                            }
                            if (order.partNumList != null)
                            {
                                foreach (CartItem cart in order.partNumList)
                                {
                                    CsOrderDetail orderDetail = new CsOrderDetail();
                                    orderDetail.OrderId = orderid;
                                    orderDetail.ProductId = cart.id;
                                    orderDetail.UnitPrice = cart.price;
                                    orderDetail.ProductNumber = cart.num;
                                    orderDetail.TotalPrice = (cart.price * cart.num);
                                    orderDetail.ChoseType = 2;
                                    detailList.Add(orderDetail);
                                    totalNumber++;
                                }
                            }
                            foreach (PartItem part in order.partList)
                            {
                                CsOrderDetail orderDetail = new CsOrderDetail();
                                orderDetail.OrderId = orderid;
                                orderDetail.ProductId = part.PartId;
                                orderDetail.UnitPrice = part.PartPrice;
                                orderDetail.ProductNumber = part.number;
                                orderDetail.TotalPrice = (part.PartPrice * part.number);
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

                            //修改用户表用户的金额和购买总量
                            StringBuilder strSql3 = new StringBuilder();
                            strSql3.Append("update CsUsers set UserBalance=UserBalance+@Balance,TotalWight=TotalWight+@Weight where UserId=@UserId ");
                            SqlParameter[] parameter3 =
                            {
                                new SqlParameter("@Balance",SqlDbType.Decimal,18),
                                new SqlParameter("@Weight",SqlDbType.Decimal,18),
                                new SqlParameter("@UserId",SqlDbType.Int,4)
                            };
                            parameter3[0].Value = (order.balancePrice - order.usebalance);//本次获得返利金额减去本次是有的余额
                            parameter3[1].Value = weight;
                            parameter3[2].Value = order.userid;
                            number += DbClient.ExecuteSql(conn, trans, strSql3.ToString(), parameter3);

                            // 添加返利记录
                            StringBuilder strSql5 = new StringBuilder();
                            strSql5.Append("insert into CsRebate (UserId,RebateMoney,RebateWeight,RebateTime) values (");
                            strSql5.Append("@UserId,@RebateMoney,@RebateWeight,@RebateTime)");
                            SqlParameter[] parameter5 =
                            {
                                new SqlParameter("@UserId",SqlDbType.Int,4),
                                new SqlParameter("@RebateMoney",SqlDbType.Decimal,18),
                                new SqlParameter("@RebateWeight",SqlDbType.Decimal,18),
                                new SqlParameter("@RebateTime",SqlDbType.DateTime)
                            };
                            parameter5[0].Value = order.userid;
                            parameter5[1].Value = order.balancePrice;
                            parameter5[2].Value = weight;
                            parameter5[3].Value = DateTime.Now;
                            number += DbClient.ExecuteSql(conn, trans, strSql5.ToString(), parameter5);
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
                        trans.Rollback();
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
        public int UpdatePrepaymentId(int orderId,string prepaymentid)
        {
            string strSql = $"update CsOrder set PrepaymentId='{prepaymentid}' where OrderId={orderId}";
            return  DbClient.Excute(strSql);
        }

        /// <summary>
        /// 根据订单编号，修改订单状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        public int UpdateOrderState(int orderid,int orderState)
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
        public List<CsOrder> GetModelListByOpenId(string openId)
        {
            var strSql = $"select a.* from CsOrder a inner join CsUsers b on a.UserId = b.UserId where a.OrderState!=0 and  b.OpenId='{openId}'";
            return DbClient.Query<CsOrder>(strSql).ToList();
        }
    }
}
