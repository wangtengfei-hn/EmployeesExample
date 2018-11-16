using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MSSQL.Repository.AggregatesModel
{
    /// <summary>
    /// 订单状态变更记录
    /// </summary>
    public class OrderStatusChange : Model
    {
        /// <summary>
        /// 订单状态OrderStatus
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Required]
        public Guid Order_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? Operator_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Operator_Remark { get; set; }
    }
}
