using Example.IBusiness;
using Example.MSSQL.Repository;
using Example.MSSQL.Repository.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Business
{
    public class AccountBusiness : IAccountBusiness
    {
        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(AccountBusiness));

        public IMSSQLDbContext dbContext { get; set; }

        /// <summary>
        /// 添加用户账户信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool AddAccount(Guid memberId)
        {
            var account = new Account
            {
                Id = memberId,
                Balance = 0,
                PasswordForPayment = null
            };

            dbContext.Account.Add(account);
            if (!dbContext.Save())
            {
                log.Error(string.Format("添加用户账户信息:memberId:{0}", memberId));
                return false;
            }

            return true;
        }
    }
}
