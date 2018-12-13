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
            //订阅广播时新建一个队列, 这样就不会影响其他队列的接收情况
            var queue = Guid.NewGuid().ToString();
            Channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: true);
            Channel.QueueBind(queue, ExchangeName, queueName);

            SubscribeData(queue, callback);
        }

        public void SubscribeQueue<T>(string queueName, Action<T> callback)
        {
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            SubscribeData(queueName, callback);
        }

        /// <summary>
        /// 订阅队列消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        private void SubscribeData<T>(string queueName, Action<T> callback)
        {
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
                    //TODO: 记录日志
                    Console.WriteLine("接收发生异常");
                    Console.WriteLine(e);
                }
                finally
                {
                    //标记为已读
                    Channel.BasicAck(deliver.DeliveryTag, false);
                }
            };

            //指定消费队列
            Channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
        }
    }
}
