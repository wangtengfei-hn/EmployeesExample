using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository.AggregatesModel
{
    /// <summary>
    /// 资金变更记录
    /// </summary>
    public class AccountRecord : Model
    {
        /// <summary>
        /// 类型 AccountRecordType
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联用户Id
        /// </summary>
        public long Member_Id { get; set; }
    }
}
