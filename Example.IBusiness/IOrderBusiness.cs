using Example.IBusiness.Model;
using System;
using System.Collections.Generic;

namespace Example.IBusiness
{
    public interface IOrderBusiness
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Contacts"></param>
        /// <param name="P_Id_Count"></param>
        /// <returns></returns>
        ResultModel<OrderModel> CreateOrder(Guid memberId, string Contacts, Dictionary<Guid, int> P_Id_Count);
        /// <summary>
        /// 获取会员订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        List<OrderModel> GetByMemberId(Guid memberId);
        /// <summary>
        /// 获取会员指定订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderModel GetByOrderId(Guid memberId, Guid orderId);
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        PaymentModel Pay(Guid memberId, Guid orderId);
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        bool Notify(Guid orderId, Guid paymentId);
    }
}
