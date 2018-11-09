using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistryLibrary.ImplementsClass
{
    /// <summary>
    /// 消息队列Event
    /// </summary>
    /// <typeparam name="T1">消息类型1</typeparam>
    /// <typeparam name="T2">消息类型2</typeparam>
    public class MessageEvent<T1, T2>
    {
        /// <summary>
        /// 实例化<see cref="MessageEvent{T1, T2}"/>
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="messageQueue">消息队列实现类</param>
        public MessageEvent(string queueName, IMessageQueue messageQueue)
        {
            QueueName = queueName;
            MessageQueue = messageQueue;
            //MessageQueue.QueueDeclare(queueName);
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
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static MessageEvent<T1, T2> operator +(MessageEvent<T1, T2> message, Func<T1, T2, Result> callback)
        {
            message.MessageQueue.Subscribe(message.QueueName, new Func<MessageEventData<T1, T2>, Result>((data) =>
            {
                return callback(data.Data1, data.Date2);
            }));
            return message;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static MessageEvent<T1, T2> operator +(MessageEvent<T1, T2> message, Action<T1, T2> callback)
        {
            message.MessageQueue.Subscribe(message.QueueName, new Action<MessageEventData<T1, T2>>((data) =>
            {
                callback(data.Data1, data.Date2);
            }));
            return message;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        public void Invoke(T1 data1, T2 data2)
        {
            MessageQueue.Publish(QueueName, new MessageEventData<T1, T2> { Data1 = data1, Date2 = data2 });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<List<Result>> InvokeAsync(T1 data1, T2 data2)
        {
            return await MessageQueue.PublishAsync(QueueName, new MessageEventData<T1, T2> { Data1 = data1, Date2 = data2 });
        }
    }
}
