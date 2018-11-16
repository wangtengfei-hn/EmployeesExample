using Example.IBusiness;
using Example.WebApi.App_Start;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("VerificationCode")]
    public class VerificationCodeController : _Controller
    {
        #region 属性注入

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(VerificationCodeController));
        /// <summary>
        /// 
        /// </summary>
        public ISMSBusiness SMSBusiness { get; set; }

        #endregion
        /// <summary>
        /// 根据手机号发送验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{phoneNumber}/Send")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        public IHttpActionResult Send([FromUri]string phoneNumber)
        {
            var result = SMSBusiness.SendCode(phoneNumber);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }
        /// <summary>
        /// 根据当前用户发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Send")]
        [MemberAuthorizeAttribute]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        public IHttpActionResult Send()
        {
            var result = SMSBusiness.SendCode(UserID);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }

    }
}