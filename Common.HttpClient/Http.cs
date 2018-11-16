using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.HttpClient
{
    public class Http
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Request_Base Get(string url)
        {
            return new Request_Base(HttpMethod.Get, url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Request_Base Post(string url)
        {
            return new Request_Base(HttpMethod.Post, url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Request_Base Put(string url)
        {
            return new Request_Base(HttpMethod.Put, url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Request_Base Patch(string url)
        {
            return new Request_Base(new HttpMethod("PATCH"), url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Request_Base Delete(string url)
        {
            return new Request_Base(HttpMethod.Delete, url);
        }
    }
}
