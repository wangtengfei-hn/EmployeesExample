using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    public class AccountRecordModel : Model
    {
        /// <summary>
        /// 类型 
        /// </summary>
        public AccountRecordType Type { get; set; }

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
        public Guid Member_Id { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public MemberModel Member { get; set; }

    }
}
