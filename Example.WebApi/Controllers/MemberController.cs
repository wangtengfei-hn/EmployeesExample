using Common;
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
    public class MemberController : _Controller
    {

        #region 属性注入

        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MemberController));
        /// <summary>
        /// 
        /// </summary>
        public IMemberBusiness MemberBusiness { get; set; }

        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(TokenModel))]
        [SwaggerResponse(400)]
        public IHttpActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = MemberBusiness.Login(model.Number, model.Password, model.Code);
            if (!result.Success)
                return BadRequest(result.Message);
            var tokenModel = new TokenModel
            {
                Member = result.Data,
                Token = AuthenticationHelper.Authenticate(result.Data.PhoneNumber, null, true, new Dictionary<string, dynamic>
                {
                    { AuthenticationHelper.UserIDKey, result.Data.Id},
                    { AuthenticationHelper.UserTicketIssueTimeKey,DateTimeOffset.Now  }
                })
            };
            //var tokenModel = new TokenModel
            //{
            //    Member = null,
            //    Token = AuthenticationHelper.Authenticate("13112345678", null, true, new Dictionary<string, dynamic>
            //    {
            //        { AuthenticationHelper.UserIDKey, Guid.NewGuid()},
            //        { AuthenticationHelper.UserTicketIssueTimeKey,DateTimeOffset.Now  }
            //    })
            //};

            return Ok(tokenModel);
        }
        /// <summary>
        /// 获取当前会员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MemberAuthorizeAttribute]
        [SwaggerResponse(200, Type = typeof(MemberModel))]
        [SwaggerResponse(400)]
        public IHttpActionResult Get()
        {
            var result = MemberBusiness.GetMemberById(UserID);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [MemberAuthorizeAttribute]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        public IHttpActionResult ModifyPassword(ModifyPasswordModel model)
        {
            var result = MemberBusiness.ModifyPassword(UserID, model.NewPassword, model.Code);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(MemberModel))]
        [SwaggerResponse(400)]
        public IHttpActionResult Register(RegisterMemberModel model)
        {
            var result = MemberBusiness.Register(model.PhoneNumber, model.NewPassword, model.Gender, model.Code);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

    }
}