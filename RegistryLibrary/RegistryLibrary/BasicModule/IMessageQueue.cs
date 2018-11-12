using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 消息队列模块
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        void Subscribe<T>(string queueName, Func<T, Result> callback);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        void Subscribe<T>(string queueName, Action<T> callback);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        void Publish<T>(string queueName, T data);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <returns>订阅者的回复结果</returns>
        Task<Result> PublishAsync<T>(string queueName, T data);
    }
}
