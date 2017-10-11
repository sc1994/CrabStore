using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DBModel
{
    public class OrderModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal totalmoney { get; set; }
       
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public decimal finalPrice { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public string orderaddress { get; set; } = string.Empty;

        /// <summary>
        /// 发货人地址
        /// </summary>
        public string sendaddress { get; set; } = string.Empty;

        /// <summary>
        /// 购买份数
        /// </summary>
        public int totalnumber { get; set; } = 1;

        /// <summary>
        /// 实际重量
        /// </summary>
        public decimal totalweight { get; set; }

        /// <summary>
        /// 寄件重量
        /// </summary>
        public decimal sendweight { get; set; }

        /// <summary>
        /// 购买螃蟹列表
        /// </summary>
        public List<CartItem> cartFoodList { get; set; }

        /// <summary>
        /// 购买可选配件列表
        /// </summary>
        public List<CartItem> partNumList { get; set; }

        /// <summary>
        /// 购买必须配件列表
        /// </summary>
        public List<PartItem> partList { get; set; }

        /// <summary>
        /// 快递费用
        /// </summary>
        public decimal expressmoney { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal servicemoney { get; set; }

        /// <summary>
        /// 是否开票
        /// </summary>
        public bool isInvoice { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string remarks { get; set; }
    }

    public class CartItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int num { get; set; } = 0;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal weight { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string tname { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public  int index { get; set; }
    }

    public class PartItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int PartId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal PartPrice { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal PartWeight { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string OperationDate { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int number { get; set; } = 0;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
