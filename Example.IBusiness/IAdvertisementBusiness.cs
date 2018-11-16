using Common;
using Example.IBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness
{
    public interface IAdvertisementBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AdvertisementModel Get(Guid id);
        /// <summary>
        /// 查GPS坐标范围内的
        /// </summary>
        /// <param name="geolocation"></param>
        /// <returns></returns>
        List<AdvertisementModel> Get(Geolocation geolocation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <param name="content"></param>
        /// <param name="geolocation"></param>
        /// <returns></returns>
        AdvertisementModel Add(string[] images, string content, Geolocation geolocation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="images"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        ResultModel Modify(Guid id, string[] images, string content);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel Delete(Guid id);
    }
}
