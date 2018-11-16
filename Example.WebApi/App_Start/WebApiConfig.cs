using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Example.WebApi.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //操作日志记录
            //config.Filters.Add(new ActionFilterAttribute());
            //异常日志记录
            //config.Filters.Add(new ExceptionHandleFilterAttribute());
            //
            config.Filters.Add(new TokenAuthorizationFilterAttribute());

            //处理多版本号访问
            //config.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(config,
            //    new KeyValuePair<string, string>("1", "Example.WebApi.V1.Controllers")
            //));

            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    "version",
            //    "v{version}/{controller}/{action}/{id}",
            //    new { id = RouteParameter.Optional },
            //    new { version = @"^(0|([1-9]\d*))(\.\d+)?$" });
            config.Routes.MapHttpRoute(
                "default",
                "{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            //配置全局返回Json数据格式
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

        }

    }
}