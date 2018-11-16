using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness
{
    public interface IAccountBusiness
    {
        /// <summary>
        /// 添加用户账户信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        bool AddAccount(Guid memberId);
    }
}
