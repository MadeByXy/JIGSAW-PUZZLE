using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;

namespace MessageQueueModule.Model
{
    /// <summary>
    /// 消费者模块
    /// </summary>
    public partial class MessageQueue
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        public void Subscribe<T>(string queueName, Func<T, Result> callback)
        {
            RpcServer.AddConsumers(queueName, (bytes) =>
            {
                var data = bytes.FromSerialization<T>();
                return callback?.Invoke(data) ?? new Result { Success = true };
            });
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        public void Subscribe<T>(string queueName, Action<T> callback)
        {
            var queue = Guid.NewGuid().ToString();
            Channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: true);
            Channel.QueueBind(queue, ExchangeName, queueName);

            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, deliver) =>
            {
                try
                {
                    var data = deliver.Body.FromSerialization<T>();
                    callback?.Invoke(data);
                }
                catch (Exception e)
                {
                    Logging.LogException("消息队列模块异常", e, $"队列名称: {queueName}");
                }
                finally
                {
                    //标记为已读
                    Channel.BasicAck(deliver.DeliveryTag, false);
                }
            };

            //指定消费队列
            Channel.BasicConsume(
                queue: queue,
                autoAck: false,
                consumer: consumer);
        }
    }
}
