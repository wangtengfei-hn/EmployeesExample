using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterMemberModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Gender { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Code { get; set; }
    }
}