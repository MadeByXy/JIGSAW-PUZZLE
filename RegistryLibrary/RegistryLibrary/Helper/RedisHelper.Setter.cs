using RegistryLibrary.Interface.Common;
using System;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// Redis 数据添加功能
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// 添加私有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(UserInfo userInfo, string key, T value)
        {
            Set(userInfo, key, value, TimeSpan.MaxValue);
        }

        /// <summary>
        /// 添加私有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="span">过期时间</param>
        public static async void Set<T>(UserInfo userInfo, string key, T value, TimeSpan span)
        {
            await PrivateDatabase.StringSetAsync(GetKey(userInfo, key), value.ToSerialization(), span);
        }

        /// <summary>
        /// 添加公共数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(string key, T value)
        {
            Set(key, value, TimeSpan.MaxValue);
        }

        /// <summary>
        /// 添加公共数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>    
        /// <param name="span">过期时间</param>
        public static async void Set<T>(string key, T value, TimeSpan span)
        {
            await PublicDatabase.StringSetAsync(GetKey(key), value.ToSerialization(), span);
        }

        /// <summary>
        /// 发布数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="channelName">频道名称</param>
        /// <param name="data">待发布数据</param>
        public static void Publish<T>(string channelName, T data)
        {
            PublicDatabase.PublishAsync(channelName, data.ToSerialization());
        }
    }
}
