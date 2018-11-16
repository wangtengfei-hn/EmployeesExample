using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// 会员充值
        /// </summary>
        [Description("会员充值")]
        Recharge = 100,

        /// <summary>
        /// 订单支付
        /// </summary>
        [Description("订单支付")]
        Order = 200,
    }
}
