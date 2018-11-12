using RegistryLibrary.Interface.Common;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Interface.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    /// <typeparam name="T1">消息类型1</typeparam>
    /// <typeparam name="T2">消息类型2</typeparam>
    public interface IEvent<T1, T2>
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        void Subscribe(Action<T1, T2> callback);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        void Subscribe(Func<T1, T2, Result> callback);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        void Invoke(T1 data1, T2 data2);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息1</param>
        /// <param name="data2">消息2</param>
        /// <returns>订阅者的回复结果</returns>
        Task<Result> InvokeAsync(T1 data1, T2 data2);
    }
}
