using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository.AggregatesModel
{
    /// <summary>
    /// 用户财产账户
    /// </summary>
    public class Account : Model
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string PasswordForPayment { get; set; }
    }
}
