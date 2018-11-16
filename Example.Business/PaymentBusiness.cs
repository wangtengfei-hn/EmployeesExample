using Common.RabbitMQ;
using Example.IBusiness;
using Example.IBusiness.Events;
using Example.IBusiness.Model;
using Example.MSSQL.Repository;
using Example.MSSQL.Repository.AggregatesModel;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Example.Business
{
    public class PaymentBusiness : IPaymentBusiness
    {
        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(PaymentBusiness));
        public IMSSQLDbContext dbContext { get; set; }
        public RabbitMQService rabbitMQService { get; set; }

        /// <summary>
        /// 选择支付方式
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentAccount"></param>
        /// <returns></returns>
        public async Task<ResultModel<string>> SelectAccount(Guid memberId, Guid paymentId, PaymentAccount paymentAccount)
        {
            var result = new ResultModel<string>();
            var payment = await dbContext.Payment.FirstOrDefaultAsync(_ => _.Id == paymentId && _.Member_Id == memberId);
            if (payment == null)
            {
                result.Message = "没有支付订单";
                return result;
            }
            if (payment.Status != (int)PaymentStatus.Created)
            {
                result.Message = "订单不能支付";
                return result;
            }

            var thirdpay = new Thirdpay
            {
                Account = (int)paymentAccount,
                PaymentId = payment.Id
            };

            switch (paymentAccount)
            {
                case PaymentAccount.Account:
                    result = await PayedByAccount(payment, thirdpay);
                    break;
                case PaymentAccount.Alipay:
                    result = new ResultModel<string>() { Data = "支付字符串" }; //alipayHelper.Pay(thirdpay.Id.ToString("N")...); thirdpay.Id.ToString("N")作为商户交易号
                    break;
                case PaymentAccount.WXPay:
                    result = new ResultModel<string>() { Data = "支付字符串" }; //wxpayHelper.Pay(thirdpay.Id.ToString("N")...);  thirdpay.Id.ToString("N")作为商户交易号
                    break;
                default:
                    break;
            }

            if (!result.Success)
                return result;

            thirdpay.Result = result.Data;
            dbContext.Thirdpay.Add(thirdpay);

            if (await dbContext.SaveAsync())
            {
                if (PaymentAccount.Account == paymentAccount)
                {
                    //发布支付消息
                    rabbitMQService.Publish<PaymentPaidEvent>(new PaymentPaidEvent { PaymentId = payment.Id, Identity_Id = payment.Identity_Id, Type = (PaymentType)payment.Type, Amount = payment.Amount }, "PaymentPaidEvent");
                }
                return result;
            }

            result.Message = "订单支付失败";
            return result;
        }
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="thirdpayId"></param>
        /// <param name="paymentAccount"></param>
        /// <returns></returns>
        public async Task<bool> Notify(Guid thirdpayId, PaymentAccount paymentAccount, string trackingNumber, string notification)
        {
            var thirdpay = await dbContext.Thirdpay.FirstOrDefaultAsync(_ => _.Id == thirdpayId);
            if (thirdpay == null)
                return false;

            thirdpay.TrackingNumber = trackingNumber;
            thirdpay.Notification = notification;

            var payment = await dbContext.Payment.FirstOrDefaultAsync(_ => _.Id == thirdpay.PaymentId);

            payment.Account = (int)paymentAccount;
            payment.Status = (int)PaymentStatus.Paid;
            payment.Thirdpay_Id = thirdpay.Id;

            if (await dbContext.SaveAsync())
            {
                //发布支付消息
                rabbitMQService.Publish<PaymentPaidEvent>(new PaymentPaidEvent { PaymentId = payment.Id, Identity_Id = payment.Identity_Id, Type = (PaymentType)payment.Type, Amount = payment.Amount }, "PaymentPaidEvent");
                return true;
            }

            return false;
        }

        private async Task<ResultModel<string>> PayedByAccount(Payment payment, Thirdpay thirdpay)
        {
            var result = new ResultModel<string>();
            var account = await dbContext.Account.FirstOrDefaultAsync(_ => _.Id == payment.Member_Id);
            if (account.Balance < payment.Amount)
            {
                result.Message = "您的余额不足";
                return result;
            }

            payment.Account = (int)PaymentAccount.Account;
            payment.Status = (int)PaymentStatus.Paid;
            var accountRecord = new AccountRecord
            {
                Amount = payment.Amount,
                Member_Id = payment.Member_Id,
                Remark = payment.Remark,
                Type = (int)AccountRecordType.Payment
            };
            payment.Thirdpay_Id = thirdpay.Id;
            result.Data = "支付成功";

            return result;
        }
    }
}
