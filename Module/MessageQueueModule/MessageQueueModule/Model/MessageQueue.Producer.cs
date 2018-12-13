using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageQueueModule.Model
{
    /// <summary>
    /// 生产者模块
    /// </summary>
    public partial class MessageQueue
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        public void Publish<T>(string queueName, T data)
        {
            Publish(queueName, data, "");
        }

        /// <summary>
        /// 发布消息到指定目标
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <param name="exchange">目标位置</param>
        public void Publish<T>(string queueName, T data, string exchange)
        {
            Channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: queueName,
                basicProperties: null,
                body: data.ToSerialization());
        }

        /// <summary>
        /// 发布队列消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <remarks>队列消息会将消息发给指定队列, 注意消息只能被接收一次</remarks>
        public void PublishQueue<T>(string queueName, T data)
        {
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            Channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: data.ToSerialization());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <param name="callback">接收订阅者的回复</param>
        public void Publish<T>(string queueName, T data, Action<Result> callback)
        {
            var result = RpcClient.Call(new BasicProperties
            {
                Headers = new Dictionary<string, object>
                {
                    { nameof(queueName), queueName }
                }
            }, data.ToSerialization());
            callback(result.Body.FromSerialization<Result>());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<Result> PublishAsync<T>(string queueName, T data)
        {
            return await Task.Run(() =>
            {
                Result returnResult = null;
                Publish(queueName, data, new Action<Result>((result) =>
                {
                    returnResult = result;
                }));
                return returnResult;
            });
        }
    }
}
