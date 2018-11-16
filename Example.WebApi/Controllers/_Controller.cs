using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Example.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class _Controller : ApiController
    {
        /// <summary>
        /// 获取已授权用户的ID
        /// </summary>
        protected Guid UserID
        {
            get
            {
                return AuthenticationHelper.GetAuthenticatedUserID(User);
            }
        }

        /// <summary>
        /// 处理绑定Model的Model验证错误
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected new IHttpActionResult BadRequest(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(_ => _.Value.Errors.Count > 0)
                .ToDictionary(_ => _.Key, _ => _.Value.Errors.First().ErrorMessage);

            return Content(System.Net.HttpStatusCode.BadRequest, errors);
        }
        /// <summary>
        /// 处理逻辑验证错误
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected new IHttpActionResult BadRequest(string error)
        {
            var errors = new Dictionary<string, string>
            {
                { "Error", error.ToString() }
            };
            return Content(System.Net.HttpStatusCode.BadRequest, errors);
        }

        /// <summary>
        /// 操作成功，不需要返回内容的操作
        /// </summary>
        /// <returns></returns>
        protected IHttpActionResult Successed()
        {
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
        /// <summary>
        /// 根据Key从Header里找值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected string GetHeadersByKey(string key)
        {
            if (Request.Headers.Contains(key))
                return Request.Headers.GetValues(key).First();
            return string.Empty;
        }
        /// <summary>
        /// 获取客户端存放于Header的数据
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected object ClientInfoByHeaders()
        {
            //终端类型
            var deviceType = 1;
            Int32.TryParse(GetHeadersByKey("device"), out deviceType);

            return new
            {
                UserToken = GetHeadersByKey("usertoken"),
                AppToken = GetHeadersByKey("token"),
                DeviceType = deviceType,
                DeviceId = GetHeadersByKey("deviceid")
            };
        }
        /// <summary>
        /// 获取客户端浏览器的原始用户代理信息
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public string GetClientUserAgent()
        {
            // Web-hosting. Needs reference to System.Web.dll
            try
            {
                if (Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    dynamic ctx = Request.Properties["MS_HttpContext"];
                    if (ctx != null)
                    {
                        return ctx.Request.UserAgent;
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }


    }
}