using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Common
{
    public class AuthenticationHelper
    {
        public static string UserTokenKey { get { return "Token"; } }
        public static string UserIDKey { get { return "ID"; } }
        public static string UserTicketIssueTimeKey { get { return "TicketIssueTime"; } }

        /// <summary>
        /// 授权用户，返回加密后的用户票据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="expireByMinutes"></param>
        /// <param name="isPersistent"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static string Authenticate(string username, int? expireByMinutes = null, bool isPersistent = false, IDictionary<string, dynamic> userData = null)
        {
            var userDataString = string.Empty;
            if (userData != null && userData.Count > 0)
                foreach (var data in userData)
                {
                    userDataString += data.Key + '=' + data.Value;
                    userDataString += "||||";
                }


            var ticket = new FormsAuthenticationTicket(0, username, DateTime.Now, isPersistent ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(expireByMinutes ?? FormsAuthentication.Timeout.Minutes), isPersistent, userDataString);

            return FormsAuthentication.Encrypt(ticket);
        }

        /// <summary>
        /// 实现票据的滑动过期
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="expireByMinutes"></param>
        /// <returns></returns>
        public static bool Sliding(ref string userToken, int? expireByMinutes = null)
        {
            try
            {
                var ticket = FormsAuthentication.Decrypt(userToken);
                if ((ticket.Expiration - ticket.IssueDate).TotalMinutes / (ticket.Expiration - DateTime.Now).TotalMinutes > 2)
                {
                    userToken = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(0, ticket.Name, DateTime.Now, ticket.IsPersistent ? DateTime.Now.AddDays(14) : DateTime.Now.AddMinutes(expireByMinutes ?? FormsAuthentication.Timeout.Minutes), ticket.IsPersistent, ticket.UserData));
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 解析已授权用户票据
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public static FormsAuthenticationTicket ResolveTicket(string userToken)
        {
            try
            {
                return FormsAuthentication.Decrypt(userToken);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 解析已授权用户票据
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public static IIdentity ResolveIdentity(string userToken)
        {
            var ticket = ResolveTicket(userToken);
            return ticket == null ? new ClaimsIdentity() : new FormsIdentity(ticket);
        }

        /// <summary>
        /// 解析已授权用户票据
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public static IPrincipal ResolvePrincipal(string userToken)
        {
            var identity = ResolveIdentity(userToken);
            return new GenericPrincipal(identity, null);
        }

        public static bool IsRemembered(IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var ticket = (user.Identity as FormsIdentity).Ticket;
                return ticket.IsPersistent;
            }
            return false;
        }

        /// <summary>
        /// 从已授权用户中获取用户数据字典
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetAuthenticatedUserData(IPrincipal user)
        {
            var userDataDic = new Dictionary<string, string>();
            if (user.Identity.IsAuthenticated)
            {
                var userDataString = (user.Identity as FormsIdentity).Ticket.UserData;
                var userData = userDataString.Split(new[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);
                if (userData.Length > 0)
                    foreach (var item in userData)
                    {
                        var data = item.Split('=');
                        var key = data[0];
                        var value = data.Length > 1 ? data[1] : null;
                        userDataDic.Add(key, value);
                    }
            }
            return userDataDic;
        }

        /// <summary>
        /// 从已授权用户中获取用户ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Guid GetAuthenticatedUserID(IPrincipal user)
        {
            var userData = GetAuthenticatedUserData(user);

            try
            {
                if (userData.ContainsKey(UserIDKey))
                    return Guid.Parse(userData[UserIDKey]);
            }
            catch { }

            return Guid.Empty;
        }
    }
}
