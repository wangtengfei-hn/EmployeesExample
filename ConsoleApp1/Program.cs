using Autofac;
using Common.RabbitMQ;
using Common.Settings;
using Example.Business;
using Example.IBusiness.Events;
using Example.MSSQL.Repository;
using Example.Redis.Repository;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {

            //开启服务处理订阅信息
            rabbitMQSubscribe();
        }
        /// <summary>
        /// 获取Json配置文件
        /// </summary>
        /// <returns></returns>
        static AppSettings GetAppSettings()
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return JsonConvert.DeserializeObject<AppSettings>(json);
        }

        static void rabbitMQSubscribe()
        {
            var appSettings = GetAppSettings();

            var mqService = new RabbitMQService(
                appSettings.RabbitMQ.HostName,
                appSettings.RabbitMQ.Port,
                appSettings.RabbitMQ.UserName,
                appSettings.RabbitMQ.Password);
            //订阅用户注册消息
            mqService.Subscribe<RegisterMemberEvent>(
                "RegisterMemberEvent",
                "RegisterMemberAddAccount",
                _ =>
                {
                    var accountBusiness = new AccountBusiness();
                    accountBusiness.dbContext = new MSSQLDbContext("MSDbConnection");
                    return accountBusiness.AddAccount(_.MemberId);
                });

            //订阅支付成功消息--订单处理
            mqService.Subscribe<PaymentPaidEvent>(
                "PaymentPaidEvent",
                "PaymentPaidOrder",
                _ =>
                {
                    if (_.Type != Example.IBusiness.Model.PaymentType.Order)
                        return true;
                    var orderBusiness = new OrderBusiness();
                    orderBusiness.dbContext = new MSSQLDbContext("MSDbConnection");
                    return orderBusiness.Notify(_.Identity_Id, _.PaymentId);
                });

            //订阅支付成功消息--充值处理
            mqService.Subscribe<PaymentPaidEvent>(
                "PaymentPaidEvent",
                "PaymentPaidRecharge",
                _ =>
                {
                    if (_.Type != Example.IBusiness.Model.PaymentType.Recharge)
                        return true;
                    //var rechargeBusiness = new RechargeBusiness();
                    //rechargeBusiness.dbContext = new MSSQLDbContext("MSDbConnection");
                    return true;
                });
        }
    }
}
