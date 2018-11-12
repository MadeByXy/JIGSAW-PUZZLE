using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Event
{
    /// <summary>
    /// 依赖于消息队列的事件模块
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class MessageEvent<T> : IEvent<T>
    {
        /// <summary>
        /// 依赖于消息队列的事件模块
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="messageQueue">消息队列实现类</param>
        public MessageEvent(string queueName, IMessageQueue messageQueue)
        {
            QueueName = queueName;
            MessageQueue = messageQueue;
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 消息队列实现类
        /// </summary>
        public IMessageQueue MessageQueue { get; private set; }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public IEvent<T> Subscribe(Action<T> callback)
        {
            MessageQueue.Subscribe(QueueName, callback);
            return this;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public IEvent<T> Subscribe(Func<T, Result> callback)
        {
            MessageQueue.Subscribe(QueueName, callback);
            return this;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息内容</param>
        public void Publish(T data)
        {
            MessageQueue.Publish(QueueName, data);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<Result> PublishAsync(T data)
        {
            return await MessageQueue.PublishAsync(QueueName, data);
        }
    }
}
