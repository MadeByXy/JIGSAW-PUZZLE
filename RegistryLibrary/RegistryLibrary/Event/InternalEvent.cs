using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Event
{
    /// <summary>
    /// 依赖于本地逻辑的事件模块
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class InternalEvent<T> : IEvent<T>
    {
        /// <summary>
        /// 依赖于本地逻辑的事件模块
        /// </summary>
        public InternalEvent() { }

        private event Func<T, Result> Events;

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void Subscribe(Action<T> callback)
        {
            Events += (data) =>
            {
                callback(data);
                return new Result { Success = true };
            };
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void Subscribe(Func<T, Result> callback)
        {
            Events += callback;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="message">事件内容</param>
        /// <param name="callback">订阅方法</param>
        /// <returns></returns>
        public static InternalEvent<T> operator +(InternalEvent<T> message, Func<T, Result> callback)
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
        public static InternalEvent<T> operator +(InternalEvent<T> message, Action<T> callback)
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
            Events?.Invoke(data);
        }

        /// <summary>
        /// 发布消息并返回结果
        /// </summary>
        /// <param name="data">消息内容</param>
        /// <returns>回复结果</returns>
        public async Task<Result> InvokeAsync(T data)
        {
            return await Task.Run(() =>
            {
                foreach (var func in Events?.GetInvocationList())
                {
                    var result = ((Func<T, Result>)func)(data);
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
