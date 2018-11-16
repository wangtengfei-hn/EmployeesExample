using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Example.WebApi.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpControllerSelector : IHttpControllerSelector
    {
        private const string VersionKey = "Version";
        private const string ControllerKey = "controller";

        private readonly HttpConfiguration _configuration;
        private readonly IDictionary<string, string> _versionNamespaceMaps;
        private readonly HashSet<string> _duplicates;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="versionNamespaceMaps"></param>
        public HttpControllerSelector(HttpConfiguration config, params KeyValuePair<string, string>[] versionNamespaceMaps)
        {
            _configuration = config;
            _versionNamespaceMaps = versionNamespaceMaps == null ? new Dictionary<string, string>() : versionNamespaceMaps.ToDictionary(_ => _.Key, _ => _.Value);
            _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            // Create a lookup table where key is "namespace.controller". The value of "namespace" is the last
            // segment of the full namespace. For example:
            // MyApplication.Controllers.V1.ProductsController => "V1.Products"
            var assembliesResolver = _configuration.Services.GetAssembliesResolver();
            var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (var type in controllerTypes)
            {
                // For the dictionary key, strip "Controller" from the end of the type name.
                // This matches the behavior of DefaultHttpControllerSelector.
                var controllerName = type.Name.Remove(type.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

                var key = type.Namespace + "." + controllerName;

                // Check for duplicate keys.
                if (dictionary.Keys.Contains(key))
                {
                    _duplicates.Add(key);
                }
                else
                {
                    dictionary[key] = new HttpControllerDescriptor(_configuration, type.Name, type);
                }
            }

            // Remove any duplicates from the dictionary, because these create ambiguous matches. 
            // For example, "Foo.V1.ProductsController" and "Bar.V1.ProductsController" both map to "v1.products".
            foreach (string s in _duplicates)
            {
                dictionary.Remove(s);
            }
            return dictionary;
        }

        // Get a value from the route data, if present.
        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            if (routeData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // 从路由或请求头中获取版本名称
            var versionName = GetRouteVariable<string>(routeData, VersionKey);
            if (string.IsNullOrWhiteSpace(versionName) || !_versionNamespaceMaps.ContainsKey(versionName))
            {
                // 从请求头中获取版本名称
                IEnumerable<string> versions;
                if (!request.Headers.TryGetValues(VersionKey, out versions))
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                if (versions == null || versions.Count() == 0)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                versionName = versions.FirstOrDefault();
                if (!_versionNamespaceMaps.ContainsKey(versionName))
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // 从版本命名空间映射中获取对应版本的命名空间
            var @namespace = _versionNamespaceMaps[versionName];
            if (@namespace == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // 从路由中获取控制器名称
            var controllerName = GetRouteVariable<string>(routeData, ControllerKey);
            if (controllerName == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // 获取控制器
            var controller = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", @namespace, controllerName);

            HttpControllerDescriptor controllerDescriptor;
            if (_controllers.Value.TryGetValue(controller, out controllerDescriptor))
                return controllerDescriptor;

            if (_duplicates.Contains(controller))
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Multiple controllers were found that match this request."));

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllers.Value;
        }
    }
}