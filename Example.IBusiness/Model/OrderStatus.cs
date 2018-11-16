using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.IBusiness.Model
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 已取消，交易关闭
        /// </summary>
        [Description("交易关闭")]
        Canceled = -100,

        /// <summary>
        /// 新订单，待付款
        /// </summary>
        [Description("待付款")]
        Created = 0,

        /// <summary>
        /// 已支付，待发货
        /// </summary>
        [Description("待发货")]
        Payed = 100,

        /// <summary>
        /// 已发货，待签收
        /// </summary>
        [Description("待签收")]
        Delivered = 200,

        /// <summary>
        /// 已签收
        /// </summary>
        [Description("已签收")]
        Signed = 300,

        /// <summary>
        /// 订单完成结束
        /// </summary>
        [Description("订单完成")]
        Complete = 1000
    }
}
