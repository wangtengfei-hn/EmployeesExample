using Common;
using Example.IBusiness;
using Example.IBusiness.Model;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("X")]
    public class AdvertisementController : _Controller
    {

        #region 属性注入

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AdvertisementController));
        /// <summary>
        /// 
        /// </summary>
        public IAdvertisementBusiness AdvertisementBusiness { get; set; }

        #endregion

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid}")]
        [SwaggerResponse(200, Type = typeof(AdvertisementModel))]
        [SwaggerResponse(404)]
        public IHttpActionResult Get([FromUri]Guid id)
        {
            var result = AdvertisementBusiness.Get(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<AdvertisementModel>))]
        public IHttpActionResult Get([FromUri]Geolocation geolocation)
        {
            var result = AdvertisementBusiness.Get(geolocation);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


    }
}