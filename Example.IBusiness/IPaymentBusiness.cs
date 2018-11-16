using Example.IBusiness.Model;
using System;
using System.Threading.Tasks;

namespace Example.IBusiness
{
    public interface IPaymentBusiness
    {
        /// <summary>
        /// 选择支付方式
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="paymentId"></param>
        /// <param name="paymentAccount"></param>
        /// <returns></returns>
        Task<ResultModel<string>> SelectAccount(Guid memberId, Guid paymentId, PaymentAccount paymentAccount);
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="thirdpayId"></param>
        /// <param name="paymentAccount"></param>
        /// <returns></returns>
        Task<bool> Notify(Guid thirdpayId, PaymentAccount paymentAccount, string trackingNumber, string notification);
    }
}
