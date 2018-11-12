using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Event
{
    /// <summary>
    /// 依赖于本地逻辑的事件模块
    /// </summary>
    /// <typeparam name="T1">消息类型1</typeparam>
    /// <typeparam name="T2">消息类型2</typeparam>
    public class InternalEvent<T1, T2> : IEvent<T1, T2>
    {
        /// <summary>
        /// 依赖于本地逻辑的事件模块
        /// </summary>
        public InternalEvent() { }

        private event Func<T1, T2, Result> Events;

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public IEvent<T1, T2> Subscribe(Action<T1, T2> callback)
        {
            Events += (data1, data2) =>
            {
                callback(data1, data2);
                return new Result { Success = true };
            };
            return this;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public IEvent<T1, T2> Subscribe(Func<T1, T2, Result> callback)
        {
            Events += callback;
            return this;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static InternalEvent<T1, T2> operator +(InternalEvent<T1, T2> message, Func<T1, T2, Result> callback)
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
        public static InternalEvent<T1, T2> operator +(InternalEvent<T1, T2> message, Action<T1, T2> callback)
        {
            message.Subscribe(callback);
            return message;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="data1">消息内容1</param>
        /// <param name="data2">消息内容2</param>
        public void Publish(T1 data1, T2 data2)
        {
            Events?.Invoke(data1, data2);
        }

        /// <summary>
        /// 发布消息并返回结果
        /// </summary>
        /// <param name="data1">消息内容1</param>
        /// <param name="data2">消息内容2</param>
        /// <returns>回复结果</returns>
        public async Task<Result> PublishAsync(T1 data1, T2 data2)
        {
            return await Task.Run(() =>
            {
                foreach (var func in Events?.GetInvocationList())
                {
                    var result = ((Func<T1, T2, Result>)func)(data1, data2);
                    if (!result.Success)
                    {
                        return result;
                    }
                }
                return new Result { Success = true };
            });
        }
    }
}
