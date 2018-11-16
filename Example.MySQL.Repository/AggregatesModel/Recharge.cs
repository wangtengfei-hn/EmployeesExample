using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository.AggregatesModel
{
    /// <summary>
    /// 用户充值
    /// </summary>
    public class Recharge : Model
    {
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public long Member_Id { get; set; }
    }
}
