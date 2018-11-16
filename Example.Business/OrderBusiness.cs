using Common.Extensions;
using Example.IBusiness;
using Example.IBusiness.Model;
using Example.MSSQL.Repository;
using Example.MSSQL.Repository.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        internal log4net.ILog log = log4net.LogManager.GetLogger(typeof(OrderBusiness));
        public IMSSQLDbContext dbContext { get; set; }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="Contacts"></param>
        /// <param name="P_Id_Count"></param>
        /// <returns></returns>
        public ResultModel<OrderModel> CreateOrder(Guid memberId, string Contacts, Dictionary<Guid, int> P_Id_Count)
        {
            var result = new ResultModel<OrderModel>();
            //P_Id_Count 商品遍历处理
            var order = new Order
            {
                ProductAmount = 100,
                Freight = 10,
                Amount = 110,
                Discount = 20,
                PaymentAmount = 90,
                Member_Id = memberId,
                Status = (int)OrderStatus.Created
            };
            dbContext.Order.Add(order);
            var orderStatus = new OrderStatusChange
            {
                Operator_Id = memberId,
                Operator_Remark = "创建订单",
                Order_Id = order.Id,
                Status = order.Status
            };
            dbContext.OrderStatusChange.Add(orderStatus);

            if (!dbContext.Save())
            {
                result.Message = "下单失败";
                return result;
            }
            result.Data = order.Translate<Order, OrderModel>((input, output) =>
            {
                output.StatusChanges = new List<OrderStatusChangeModel> { orderStatus.Translate<OrderStatusChange, OrderStatusChangeModel>() };
            });
            return result;
        }
        /// <summary>
        /// 获取会员订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OrderModel> GetByMemberId(Guid memberId)
        {
            var orders = dbContext.Order.Where(_ => _.Member_Id == memberId).ToList();

            return orders.Select(_ => _.Translate<Order, OrderModel>()).ToList();
        }
        /// <summary>
        /// 获取会员指定订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderModel GetByOrderId(Guid memberId, Guid orderId)
        {
            var order = dbContext.Order.FirstOrDefault(_ => _.Member_Id == memberId && _.Id == orderId);
            if (order == null)
                return null;
            var orderStatusChanges = dbContext.OrderStatusChange.Where(_ => _.Order_Id == orderId).ToList();
            var result = order.Translate<Order, OrderModel>();
            result.StatusChanges = orderStatusChanges.Select(_ => _.Translate<OrderStatusChange, OrderStatusChangeModel>()).ToList();
            return result;
        }
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public PaymentModel Pay(Guid memberId, Guid orderId)
        {
            var order = dbContext.Order.FirstOrDefault(_ => _.Member_Id == memberId && _.Id == orderId);
            if (order == null)
                return null;
            var payment = dbContext.Payment.FirstOrDefault(_ => _.Identity_Id == order.Id);
            if (payment == null)
            {
                payment = new Payment
                {
                    Amount = order.PaymentAmount,
                    Amount_ByAccount = 0,
                    Amount_ByThirdpay = 0,
                    Member_Id = order.Member_Id,
                    Identity_Id = order.Id,
                    Type = (int)PaymentType.Order,
                    Status = (int)PaymentStatus.Created,
                    Account = (int)(PaymentAccount.Account | PaymentAccount.Alipay | PaymentAccount.WXPay),
                    Remark = "支付订单：" + order.Id
                };
                dbContext.Payment.Add(payment);
                if (!dbContext.Save())
                {
                    return null;
                }
            }

            return payment.Translate<Payment, PaymentModel>();
        }
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public bool Notify(Guid orderId, Guid paymentId)
        {
            var order = dbContext.Order.FirstOrDefault(_ => _.Id == orderId);
            if (order == null)
            {
                log.Error(string.Format("支付通知找不到订单：paymentId：{0}，orderId：{1}", paymentId, orderId));
                return true;
            }
            if (order.Status != (int)OrderStatus.Created)
            {
                log.Error(string.Format("支付通知订单状态已变更：paymentId：{0}，orderId：{1}", paymentId, orderId));
                return true;
            }
            order.Status = (int)OrderStatus.Payed;
            if (!dbContext.Save())
            {
                log.Error(string.Format("支付通知订单状态更新失败：paymentId：{0}，orderId：{1}", paymentId, orderId));
                return false;
            }
            return true;
        }
    }
}
