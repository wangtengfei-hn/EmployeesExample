using Example.IBusiness.Model;
using Example.Redis.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Business.Service
{
    internal class SMSService
    {
        readonly string REDIS_Key = "REDIS_SmsVerificationCode";
        readonly string DAY_Key = DateTime.Now.ToString("yyyyMMdd");
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        internal ResultModel Send(string mobile, string content)
        {
            var result = new ResultModel();
            content = content + "【签名】";
            //发送验证码

            //发送成功

            result.Success = true;
            return result;
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ResultModel SendCode(RedisContext redisContext, string mobile)
        {
            var result = new ResultModel();
            //判断是否为手机号
            //if (string.IsNullOrEmpty(mobile) || !ValidateHelper.IsMobile(mobile))
            //{
            //    result.Success = false;
            //    result.Message = "请输入正确的手机号";
            //    return result;
            //}
            var smsCode = this.GetSmsVerificationCode(redisContext, mobile);
            //判断是否可以继续发送，默认有效期5分钟，发送时间间隔2分钟
            if (smsCode != null && DateTime.Now < smsCode.Expire.AddMinutes(-3))
            {
                result.Success = false;
                result.Message = "信验证码发送失败";
                return result;
            }
            //
            var sendCount = 0;
            if (smsCode != null)
                sendCount = smsCode.SendCount;
            //一个手机号只能连续发送10次短信一天有效期
            if (sendCount >= 10)
            {
                result.Success = false;
                result.Message = "信验证码发送失败";
                return result;
            }

            var code = GetCode(6);
            this.SetSmsVerificationCode(redisContext, mobile, new SmsVerificationCode { Code = code, Expire = DateTime.Now.AddMinutes(5), SendCount = sendCount + 1 });
            var content = string.Format("验证码：{0}", code);
            return Send(mobile, content);
        }

        /// <summary>
        /// 校验短信验证码
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="remove">验证通过时是否从数据库移除</param>
        /// </summary>
        /// <returns></returns>
        public ResultModel VerificationCode(RedisContext redisContext, string mobile, string code, bool remove = true)
        {
            var result = new ResultModel();
            var smsCode = GetSmsVerificationCode(redisContext, mobile);

            if (smsCode == null)
            {
                result.Message = "请获取验证码";
                return result;
            }
            if (smsCode.Code != code || DateTime.Now > smsCode.Expire)
            {
                result.Message = "验证码输入错误";
                return result;
            }

            if (remove)
                RemoveByRedis(redisContext, mobile);

            result.Success = true;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisContext"></param>
        /// <param name="mobile"></param>
        /// <param name="model"></param>
        private void SetSmsVerificationCode(RedisContext redisContext, string mobile, SmsVerificationCode model)
        {
            redisContext.Set<SmsVerificationCode>(REDIS_Key, mobile + DAY_Key, model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisContext"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private SmsVerificationCode GetSmsVerificationCode(RedisContext redisContext, string mobile)
        {
            return redisContext.Get<SmsVerificationCode>(REDIS_Key, mobile + DAY_Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisContext"></param>
        /// <param name="mobile"></param>
        private void RemoveByRedis(RedisContext redisContext, string mobile)
        {
            redisContext.Remove(REDIS_Key, mobile + DAY_Key);
        }

        /// <summary>
        /// 获取随机数字
        /// </summary>
        /// <param name="intSize"></param>
        /// <returns></returns>
        private string GetCode(int intSize)
        {
            return "123456";
            string code = "";
            var random = new Random();
            char[] codes = "0123456789".ToCharArray();
            for (int i = 0; i < intSize; i++)
            {
                code += codes[random.Next(0, codes.Length)].ToString();
            }
            return code;
        }
    }

    public class SmsVerificationCode
    {
        public string Code { get; set; }
        public int SendCount { get; set; }
        public DateTime Expire { get; set; }
    }

}
