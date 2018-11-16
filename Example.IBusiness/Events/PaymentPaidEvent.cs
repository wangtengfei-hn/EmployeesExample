using Example.IBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Events
{
    public class PaymentPaidEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid PaymentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Identity_Id { get; set; }
        /// <summary>
        /// 标识 
        /// </summary>
        public PaymentType Type { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
