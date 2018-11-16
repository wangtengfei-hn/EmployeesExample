using Common;
using Example.IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
                return false;

            if (!AuthenticationHelper.IsRemembered(actionContext.RequestContext.Principal))
                return true;

            return IsAuthenticatedStrictly(actionContext.RequestContext.Principal);
        }

        bool IsAuthenticatedStrictly(IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return false;


            return true;
            //try
            //{
            //    var userData = AuthenticationHelper.GetAuthenticatedUserData(user);
            //    var userID = Guid.Parse(userData[AuthenticationHelper.UserIDKey]);
            //    var ticketIssueTime = DateTimeOffset.Parse(userData[AuthenticationHelper.UserTicketIssueTimeKey]);

            //    var business = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IMemberBusiness)) as IMemberBusiness;
            //        var u = business.GetMemberById(userID);
            //    if (u == null)
            //        return false;
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}

        }

    }
}