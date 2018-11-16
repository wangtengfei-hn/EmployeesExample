using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Example.WebApi.App_Start
{
    /// <summary>
    /// 验证用户是否登录
    /// </summary>
    public class TokenAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public const string AuthorizationHeaderKey = "Token";
        /// <summary>
        /// 验证用户Token
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            IEnumerable<string> userTokens;
            if (!headers.TryGetValues(AuthorizationHeaderKey, out userTokens))
                return;

            var userToken = userTokens.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(userToken))
                return;

            //实现票据的滑动过期
            if (AuthenticationHelper.Sliding(ref userToken, 7 * 24 * 60))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    NewToken = userToken
                });
            }
            else
                actionContext.RequestContext.Principal = AuthenticationHelper.ResolvePrincipal(userToken);

            base.OnAuthorization(actionContext);
        }
    }
}