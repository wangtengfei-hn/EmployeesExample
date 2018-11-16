using Example.IBusiness;
using Example.IBusiness.Model;
using Example.WebApi.App_Start;
using Example.WebApi.Models;
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
    [MemberAuthorizeAttribute]
    public class OrderController : _Controller
    {

        #region 属性注入

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(OrderController));
        /// <summary>
        /// 
        /// </summary>
        public IOrderBusiness OrderBusiness { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MemberAuthorizeAttribute]
        [SwaggerResponse(200, Type = typeof(IEnumerable<OrderModel>))]
        public IHttpActionResult Get()
        {
            var result = OrderBusiness.GetByMemberId(UserID);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">orderId</param>
        /// <returns></returns>
        [HttpGet]
        [MemberAuthorizeAttribute]
        [SwaggerResponse(200, Type = typeof(OrderModel))]
        [SwaggerResponse(404)]
        public IHttpActionResult GetByOrderId([FromUri]Guid id)
        {
            var result = OrderBusiness.GetByOrderId(UserID, id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(OrderModel))]
        [SwaggerResponse(400)]
        public IHttpActionResult Create(CreateOrderModel model)
        {
            var result = OrderBusiness.CreateOrder(UserID, model.Contacts, model.P_Id_Count);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">orderId</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(PaymentModel))]
        [SwaggerResponse(404)]
        public IHttpActionResult Pay([FromUri]Guid id)
        {
            var result = OrderBusiness.Pay(UserID, id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


    }
}