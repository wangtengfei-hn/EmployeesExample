using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MSSQL.Repository.AggregatesModel
{
    /// <summary>
    /// 第三方支付信息
    /// </summary>
    public class Thirdpay : Model
    {
        /// <summary>
        /// 第三方支付流水号
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 通知返回信息
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// 支付账户 PaymentAccount
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// 支付ID
        /// </summary>
        public Guid PaymentId { get; set; }

    }
}
