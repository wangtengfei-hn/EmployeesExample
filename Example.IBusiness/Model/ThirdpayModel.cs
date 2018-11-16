using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class ThirdpayModel : Model
    {
        /// <summary>
        /// true:退款，false:付款
        /// </summary>
        public bool Identity { get; set; }

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
        /// 支付账户
        /// </summary>
        public PaymentAccount Account { get; set; }

        /// <summary>
        /// 付款信息
        /// </summary>
        public PaymentModel Payment { get; set; }
    }
}
