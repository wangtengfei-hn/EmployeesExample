using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        Created = 100,

        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paid = 200
    }
}
