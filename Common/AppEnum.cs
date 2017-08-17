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

    public enum ProductType
    {
        大宗采购 = 1,
        包塘直补 = 2
    }

    public enum ChoseType
    {
        螃蟹 = 1,
        配件 = 2
    }

    public enum ResStatue
    {
        Yes = 1,
        No = 0,
        Warn = 2,
        LoginOut = 3
    }

    public enum SysUserType
    {
        管理员 = 1,
        普通用户 = 2
    }

    public enum PartType
    {
        必选配件 = 1,
        可选配件 = 2
    }

    public enum ProductState
    {
        在售 = 1,
        已下架 = 0
    }
}
