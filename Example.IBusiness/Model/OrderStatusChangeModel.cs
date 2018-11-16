using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class OrderStatusChangeModel : Model
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid Order_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? Operator_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Operator_Remark { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderModel Order { get; set; }
    }
}
