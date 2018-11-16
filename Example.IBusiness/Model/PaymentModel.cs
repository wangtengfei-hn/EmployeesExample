using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class PaymentModel : Model
    {
        /// <summary>
        /// 标识 
        /// </summary>
        public PaymentType Type { get; set; }

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
        /// 支付账户
        /// </summary>
        public PaymentAccount Account { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public PaymentStatus Status { get; set; }

        /// <summary>
        /// 第三方支付信息Id
        /// </summary>
        public Guid? Thirdpay_Id { get; set; }

        /// <summary>
        /// 所属用户Id
        /// </summary>
        public Guid Member_Id { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public MemberModel Member { get; set; }
        /// <summary>
        /// 第三方信息
        /// </summary>
        public ThirdpayModel Thirdpay { get; set; }
    }
}
