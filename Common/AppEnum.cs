namespace Common
{
    public enum OrderState
    {
        取消订单 = 0,
        已下单未支付 = 1,
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
        蟹塘直采 = 2,
        套餐 = 3,
    }

    public enum ChoseType
    {
        螃蟹 = 1,
        配件 = 2,
        套餐 = 3
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
        可选配件 = 2,
    }

    public enum ProductState
    {
        在售 = 1,
        已下架 = 0
    }

    public enum OrderEnum
    {
        // ReSharper disable once InconsistentNaming
        ASC = 1,
        // ReSharper disable once InconsistentNaming
        DESC = 0
    }

    public enum ExcelRow
    {
        Empty,
        Order,
        Detail,
        Other
    }

    /// <summary>
    /// execl 中的 bool 值类型
    /// </summary>
    public enum ExcelBool
    {
        Y,
        N
    }
}
