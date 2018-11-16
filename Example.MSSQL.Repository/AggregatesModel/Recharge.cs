using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MSSQL.Repository.AggregatesModel
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
        [Required]
        public Guid Member_Id { get; set; }
    }
}
