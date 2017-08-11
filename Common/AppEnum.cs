namespace Common
{
    public enum OrderState
    {
        取消订单 = 0,
        已下单未至支付 = 1,
        支付成功 = 2,
        配货中 = 3,
        已发货 = 4
    }

    public enum RowStatus
    {
        有效 = 1,
        无效 = 0
    }
}
