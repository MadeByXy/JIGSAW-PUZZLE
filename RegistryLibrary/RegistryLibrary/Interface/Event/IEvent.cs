using RegistryLibrary.Interface.Common;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Interface.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IEvent<T>
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        void Subscribe(Action<T> callback);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        void Subscribe(Func<T, Result> callback);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息内容</param>
        void Invoke(T data);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data">消息</param>
        /// <returns>订阅者的回复结果</returns>
        Task<Result> InvokeAsync(T data);
    }
}
