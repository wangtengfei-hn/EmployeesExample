using Example.IBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness
{
    public interface ISMSBusiness
    {
        /// <summary>
        /// 根据手机号发送
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        ResultModel SendCode(string phone);
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ResultModel VerificationCode(string phone, string code);
        /// <summary>
        /// 根据用户Id发送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel SendCode(Guid id);
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ResultModel VerificationCode(Guid id, string code);

    }
}
