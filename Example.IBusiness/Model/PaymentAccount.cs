using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    /// <summary>
    /// 支付账户
    /// </summary>
    public enum PaymentAccount
    {
        /// <summary>
        /// 余额
        /// </summary>
        [Description("余额")]
        Account = 1,

        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 2,

        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WXPay = 4,
    }
}
