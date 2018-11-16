using Example.IBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness
{
    public interface IMemberBusiness
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ResultModel<MemberModel> Login(string phone, string password, string code);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="Gender"></param>
        /// <returns></returns>
        ResultModel<MemberModel> Register(string phone, string password, bool? gender, string code);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel<MemberModel> GetMemberById(Guid id);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        ResultModel<MemberModel> GetMemberByPhone(string phone);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ResultModel ModifyPassword(Guid id, string password, string code);

    }
}
