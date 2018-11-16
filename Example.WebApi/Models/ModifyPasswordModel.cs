using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ModifyPasswordModel
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}