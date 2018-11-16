using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class OrderModel : Model
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
        /// 订单状态 
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public Guid Member_Id { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public MemberModel Member { get; set; }
        /// <summary>
        /// 状态变更记录
        /// </summary>
        public List<OrderStatusChangeModel> StatusChanges { get; set; }
    }
}
