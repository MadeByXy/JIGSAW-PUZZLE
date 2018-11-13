using RegistryLibrary.Interface.Common;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// Redis 数据获取功能
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// 获取私有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(UserInfo userInfo, string key)
        {
            var data = await PrivateDatabase.StringGetAsync(GetKey(userInfo, key));
            if (data == RedisValue.Null)
            {
                return default(T);
            }
            else
            {
                return ((byte[])data).FromSerialization<T>();
            }
        }

        /// <summary>
        /// 获取公有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string key)
        {
            var data = await PublicDatabase.StringGetAsync(GetKey(key));
            if (data == RedisValue.Null)
            {
                return default(T);
            }
            else
            {
                return ((byte[])data).FromSerialization<T>();
            }
        }

        /// <summary>
        /// 订阅数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="channelName">订阅频道</param>
        /// <param name="callback">回调方法</param>
        public static void Subscribe<T>(string channelName, Action<T> callback)
        {
            Subscriber.Subscribe(channelName, (channel, data) =>
            {
                callback?.Invoke(((byte[])data).FromSerialization<T>());
            });
        }
    }
}
