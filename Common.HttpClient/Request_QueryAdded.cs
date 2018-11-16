using System.Collections.Generic;

namespace Common.HttpClient
{
    /// <summary>
    /// 
    /// </summary>
    public class Request_QueryAdded : Request_HeaderAdded
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Request_QueryAdded AddQuery(string name, string value)
        {
            Queries.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }
    }
}
