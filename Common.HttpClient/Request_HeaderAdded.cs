using System.Collections.Generic;

namespace Common.HttpClient
{
    /// <summary>
    /// 
    /// </summary>
    public class Request_HeaderAdded : Request_ContentAdded
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Request_HeaderAdded AddHeader(string name, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }
    }
}