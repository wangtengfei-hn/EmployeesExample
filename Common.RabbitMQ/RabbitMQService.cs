using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{

    public class RabbitMQService : IDisposable
    {
        const string BROKER_NAME = "Example";
        readonly RabbitMQPersistentConnection persistentConnection;

        public RabbitMQService(string hostName, int port, string userName, string password)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
            };

            this.persistentConnection = new RabbitMQPersistentConnection(factory);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <param name="routingKey">消息Key--将消息插入所有绑定当前Key的队列</param>
        /// <param name="queueName">队列名--默认不开启队列当前Key下没有订阅者时消息将被丢弃.传入值时将开启队列</param>
        public void Publish<T>(T @event, string routingKey, string queueName = null)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            using (var channel = persistentConnection.CreateModel())
            {
                //声明路由
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);
                //声明队列，绑定消息
                if (!string.IsNullOrEmpty(queueName))
                {
                    channel.QueueDeclare(queue: queueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                    channel.QueueBind(queue: queueName, exchange: BROKER_NAME, routingKey: routingKey);
                }
                //处理消息
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                //持久化
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                //发布信息
                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">消息Key</param>
        /// <param name="handlers">消费处理委托</param>
        public void Subscribe<T>(string queueName, string routingKey, params Func<T, bool>[] handlers)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }
            //
            var channel = persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);
            //声明队列
            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            //绑定消息
            channel.QueueBind(
                queue: queueName,
                exchange: BROKER_NAME,
                routingKey: routingKey);
            //处理机制
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            //绑定订阅者处理委托
            var consumer = new EventingBasicConsumer(channel);
            foreach (var handler in handlers)
            {
                consumer.Received += (model, ea) =>
                {
                    var eventName = ea.RoutingKey;
                    var message = Encoding.UTF8.GetString(ea.Body);

                    var integrationEvent = JsonConvert.DeserializeObject<T>(message);
                    var result = handler(integrationEvent);
                    if (result)
                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    else
                        channel.BasicNack(ea.DeliveryTag, false, true);
                };
            }
            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

        }

        public void Dispose()
        {
            if (persistentConnection != null)
            {
                persistentConnection.Dispose();
            }
        }


    }
}
