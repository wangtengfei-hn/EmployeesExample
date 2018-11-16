using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Example.WebApi.ExtendMethods
{
    public static class ExtendMethods
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage =
            "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            // Web-hosting. Needs reference to System.Web.dll
            try
            {
                if (request.Properties.ContainsKey(HttpContext))
                {
                    dynamic ctx = request.Properties[HttpContext];
                    if (ctx != null)
                    {
                        return ctx.Request.UserHostAddress;
                    }
                }

                // Self-hosting. Needs reference to System.ServiceModel.dll. 
                if (request.Properties.ContainsKey(RemoteEndpointMessage))
                {
                    dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                    if (remoteEndpoint != null)
                    {
                        return remoteEndpoint.Address;
                    }
                }

                // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
                if (request.Properties.ContainsKey(OwinContext))
                {
                    dynamic owinContext = request.Properties[OwinContext];
                    if (owinContext != null)
                    {
                        return owinContext.Request.RemoteIpAddress;
                    }
                }
            }
            catch (Exception)
            {
            }

            return "127.0.0.1";
        }

        /// <summary>
        /// 获取头信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetHeaderString(this HttpRequestMessage request)
        {
            var header = new StringBuilder();

            foreach (var item in request.Headers.ToArray())
            {
                header.AppendFormat("{0} : {1}", item.Key, string.Join(",", item.Value));
                header.AppendLine();
            }
            return header.ToString();
        }

    }
}