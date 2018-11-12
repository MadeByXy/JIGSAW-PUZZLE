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
    /// <typeparam name="T1">消息类型1</typeparam>
    /// <typeparam name="T2">消息类型2</typeparam>
    public class MessageEvent<T1, T2> : IEvent<T1, T2>
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
        public IEvent<T1, T2> Subscribe(Action<T1, T2> callback)
        {
            MessageQueue.Subscribe(QueueName, (MessageEventData<T1, T2> data) =>
            {
                callback(data.Data1, data.Date2);
            });
            return this;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public IEvent<T1, T2> Subscribe(Func<T1, T2, Result> callback)
        {
            MessageQueue.Subscribe(QueueName, (MessageEventData<T1, T2> data) =>
            {
                return callback(data.Data1, data.Date2);
            });
            return this;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        public void Publish(T1 data1, T2 data2)
        {
            MessageQueue.Publish(QueueName, new MessageEventData<T1, T2> { Data1 = data1, Date2 = data2 });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<Result> PublishAsync(T1 data1, T2 data2)
        {
            return await MessageQueue.PublishAsync(QueueName, new MessageEventData<T1, T2> { Data1 = data1, Date2 = data2 });
        }
    }
}
