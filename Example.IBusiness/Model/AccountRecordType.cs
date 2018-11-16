using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    /// <summary>
    /// 账户变更类型
    /// </summary>
    public enum AccountRecordType
    {
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 100,

        /// <summary>
        /// 取现
        /// </summary>
        [Description("取现")]
        Cashout = -100,

        /// <summary>
        /// 转入
        /// </summary>
        [Description("转入")]
        TransferIn = 200,

        /// <summary>
        /// 转出
        /// </summary>
        [Description("转出")]
        TransferOut = -200,

        /// <summary>
        /// 支付
        /// </summary>
        [Description("支付")]
        Payment = -300,

        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 300,
    }
}
