using System.IO;
using System.Net.Http;
using System.Text;

namespace Common.HttpClient
{
    /// <summary>
    /// 
    /// </summary>
    public class Request_ContentAdded : Request
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Request_ContentAdded AddContent(HttpContent content)
        {
            Contents.Add(content);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Request_ContentAdded AddContent(string content) => AddContent(new StringContent(content));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Request_ContentAdded AddContent(string content, Encoding encoding) => AddContent(new StringContent(content, encoding));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public Request_ContentAdded AddContent(string content, Encoding encoding, string mediaType) => AddContent(new StringContent(content, encoding, mediaType));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Request_ContentAdded AddContent(Stream content) => AddContent(new StreamContent(content));
    }
}