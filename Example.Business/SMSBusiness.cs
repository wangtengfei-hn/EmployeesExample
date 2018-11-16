using Example.Business.Service;
using Example.IBusiness;
using Example.IBusiness.Model;
using Example.MSSQL.Repository;
using Example.Redis.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Business
{
    public class SMSBusiness : ISMSBusiness
    {
        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(SMSBusiness));
        public RedisContext redisContext { get; set; }
        public IMSSQLDbContext dbContext { get; set; }

        /// <summary>
        /// 根据手机号发送
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ResultModel SendCode(string phone)
        {
            return new SMSService().SendCode(redisContext, phone); ;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResultModel VerificationCode(string phone, string code)
        {
            return new SMSService().VerificationCode(redisContext, phone, code);
        }
        /// <summary>
        /// 根据用户Id发送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel SendCode(Guid id)
        {
            var member = dbContext.Member.AsNoTracking().FirstOrDefault(_ => _.Id == id);
            return new SMSService().SendCode(redisContext, member.PhoneNumber);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResultModel VerificationCode(Guid id, string code)
        {
            var member = dbContext.Member.AsNoTracking().FirstOrDefault(_ => _.Id == id);
            return new SMSService().VerificationCode(redisContext, member.PhoneNumber, code);
        }
    }
}
