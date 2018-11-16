using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class LoginModel
    {
        public string Number { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}