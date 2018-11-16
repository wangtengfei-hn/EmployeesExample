using Example.IBusiness;
using Example.IBusiness.Model;
using Example.WebApi.App_Start;
using Example.WebApi.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentController : _Controller
    {

        #region 属性注入

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PaymentController));
        /// <summary>
        /// 
        /// </summary>
        public IPaymentBusiness PaymentBusiness { get; set; }

        #endregion

        /// <summary>
        /// 继续完成付款
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        [SwaggerResponse(200, Type = typeof(string))]
        [SwaggerResponse(400)]
        [MemberAuthorizeAttribute]
        public async Task<IHttpActionResult> Patch([FromUri]Guid id, [FromUri]PaymentAccount paymentAccount)
        {
            var result = await PaymentBusiness.SelectAccount(UserID, id, paymentAccount);
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AlipayNotify()
        {
            //获取参数
            //判断交易状态 是否是支付成功通知
            //验证
            //获取商户交易号 thirdpay.Id
            var notification = "三方请求参数字符串";
            var trackingNumber = "三方交易号";
            var thirdpayId = Guid.Parse("商户交易号");

            var result = await PaymentBusiness.Notify(thirdpayId, PaymentAccount.Alipay, trackingNumber, notification);
            if (result)
                return "success";

            return "fail";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXNotify()
        {
            //获取参数
            //判断交易状态 是否是支付成功通知
            //验证
            //获取商户交易号 thirdpay.Id
            var notification = "三方请求参数字符串";
            var trackingNumber = "三方交易号";
            var thirdpayId = Guid.Parse("商户交易号");

            var result = await PaymentBusiness.Notify(thirdpayId, PaymentAccount.WXPay, trackingNumber, notification);
            if (result)
                return "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";

            return null;
        }
    }
}