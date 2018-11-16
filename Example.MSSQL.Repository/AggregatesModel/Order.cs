using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MSSQL.Repository.AggregatesModel
{
    /// <summary>
    /// 用户订单
    /// </summary>
    public class Order : Model
    {
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单支付总额
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// 商品总额
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 订单优惠
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 订单状态 OrderStatus
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Required]
        public Guid Member_Id { get; set; }

    }
}
