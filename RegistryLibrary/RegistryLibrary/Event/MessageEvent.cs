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
        public void Subscribe(Action<T> callback)
        {
            MessageQueue.Subscribe(QueueName, callback);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void Subscribe(Func<T, Result> callback)
        {
            MessageQueue.Subscribe(QueueName, callback);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static MessageEvent<T> operator +(MessageEvent<T> message, Func<T, Result> callback)
        {
            message.Subscribe(callback);
            return message;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static MessageEvent<T> operator +(MessageEvent<T> message, Action<T> callback)
        {
            message.Subscribe(callback);
            return message;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息内容</param>
        public void Invoke(T data)
        {
            MessageQueue.Publish(QueueName, data);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<Result> InvokeAsync(T data)
        {
            return await MessageQueue.PublishAsync(QueueName, data);
        }
    }
}
