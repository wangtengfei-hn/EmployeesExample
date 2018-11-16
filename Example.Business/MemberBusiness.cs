using Common.Extensions;
using Common.RabbitMQ;
using Example.Business.Service;
using Example.IBusiness;
using Example.IBusiness.Events;
using Example.IBusiness.Model;
using Example.MSSQL.Repository;
using Example.MSSQL.Repository.AggregatesModel;
using Example.Redis.Repository;
using System;
using System.Linq;

namespace Example.Business
{
    public class MemberBusiness : IMemberBusiness
    {
        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(MemberBusiness));
        public IMSSQLDbContext dbContext { get; set; }
        public RedisContext redisContext { get; set; }
        public RabbitMQService rabbitMQService { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResultModel<MemberModel> Login(string phone, string password, string code)
        {
            var result = new ResultModel<MemberModel>();
            var verificationCode = new SMSService().VerificationCode(redisContext, phone, code);
            if (!verificationCode.Success)
            {
                result.Message = verificationCode.Message;
                return result;
            }
            //AsNoTracking不追踪实体状态，只用于查询
            var member = dbContext.Member.AsNoTracking().FirstOrDefault(_ => _.PhoneNumber == phone && _.Password == password);
            if (member == null)
            {
                result.Message = "账号或密码错误";
                return result;
            }
            result.Data = member.Translate<Member, MemberModel>();
            return result;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="Gender"></param>
        /// <returns></returns>
        public ResultModel<MemberModel> Register(string phone, string password, bool? gender, string code)
        {
            var result = new ResultModel<MemberModel>();
            var verificationCode = new SMSService().VerificationCode(redisContext, phone, code);
            if (!verificationCode.Success)
            {
                result.Message = verificationCode.Message;
                return result;
            }

            var member = new Member
            {
                Gender = gender,
                Password = password,
                PhoneNumber = phone
            };
            dbContext.Member.Add(member);
            if (!dbContext.Save())
            {
                result.Message = "注册失败";
                return result;
            }
            result.Data = member.Translate<Member, MemberModel>();
            //发布用户注册消息
            rabbitMQService.Publish<RegisterMemberEvent>(new RegisterMemberEvent { MemberId = member.Id, PhoneNumber = member.PhoneNumber }, "RegisterMemberEvent");

            return result;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel<MemberModel> GetMemberById(Guid id)
        {
            var result = new ResultModel<MemberModel>();
            var member = dbContext.Member.AsNoTracking().FirstOrDefault(_ => _.Id == id);
            if (member == null)
                return result;
            result.Data = member.Translate<Member, MemberModel>();
            return result;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ResultModel<MemberModel> GetMemberByPhone(string phone)
        {
            var result = new ResultModel<MemberModel>();
            var member = dbContext.Member.AsNoTracking().FirstOrDefault(_ => _.PhoneNumber == phone );
            if (member == null)
                return result;
            result.Data = member.Translate<Member, MemberModel>();
            return result;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResultModel ModifyPassword(Guid id, string password, string code)
        {
            var result = new ResultModel();
            var member = dbContext.Member.FirstOrDefault(_ => _.Id == id);
            var verificationCode = new SMSService().VerificationCode(redisContext, member.PhoneNumber, code);
            if (!verificationCode.Success)
            {
                result.Message = verificationCode.Message;
                return result;
            }
            member.Password = password;
            if (!dbContext.Save())
            {
                result.Message = "修改密码失败";
                return result;
            }
            result.Success = true;
            return result;
        }

    }
}
