using Example.IBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class TokenModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public MemberModel Member { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

    }
}