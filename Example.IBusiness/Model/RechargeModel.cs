using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class RechargeModel
    {
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public Guid Member_Id { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public MemberModel Member { get; set; }
    }
}
