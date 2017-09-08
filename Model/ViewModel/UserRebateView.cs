using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class UserRebateView
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal UserBalance { get; set; }
        /// <summary>
        /// 用户购买总重量
        /// </summary>
        public decimal TotalWight { get; set; }

        /// <summary>
        /// 用户获取返利
        /// </summary>
        public decimal RebateMoney { get; set; }

        /// <summary>
        /// 用户已经使用返利
        /// </summary>
        public decimal DiscountMoney { get; set; }
    }
}
