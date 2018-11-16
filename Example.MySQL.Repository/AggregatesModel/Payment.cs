using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository.AggregatesModel
{
    /// <summary>
    /// 用户支付
    /// </summary>
    public class Payment :Model
    {
        /// <summary>
        /// 标识 PaymentType
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 余额支付金额
        /// </summary>
        public decimal Amount_ByAccount { get; set; }

        /// <summary>
        /// 第三方支付金额
        /// </summary>
        public decimal Amount_ByThirdpay { get; set; }

        /// <summary>
        /// 支付账户PaymentAccount
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// 状态PaymentStatus
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 第三方支付信息Id
        /// </summary>
        public long? Thirdpay_Id { get; set; }

        /// <summary>
        /// 所属用户Id
        /// </summary>
        public long Member_Id { get; set; }
    }
}
