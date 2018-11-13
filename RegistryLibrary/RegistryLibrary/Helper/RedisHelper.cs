using RegistryLibrary.Interface.Common;
using StackExchange.Redis;
using System;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// 公共仓库
        /// </summary>
        private static IDatabase PublicDatabase { get; set; }

        /// <summary>
        /// 私人仓库
        /// </summary>
        private static IDatabase PrivateDatabase { get; set; }

        private static ISubscriber Subscriber { get; set; }
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        private static TimeSpan ExpiryTime { get; set; } = new TimeSpan(2, 0, 0);

        /// <summary>
        /// Redis服务器
        /// </summary>
        private static string RedisHost { get; set; } = "localhost:6379";

        /// <summary>
        /// Redis服务器密码
        /// </summary>
        private static string RedisPassWord { get; set; } = "dysoft-redis";

        private RedisHelper()
        {
            var redis = ConnectionMultiplexer.Connect($"{RedisHost},password={RedisPassWord}");
            PublicDatabase = redis.GetDatabase(4);
            PrivateDatabase = redis.GetDatabase(5);
            Subscriber = redis.GetSubscriber();
        }

        /// <summary>
        /// <see cref="RedisHelper"/>的唯一实例
        /// </summary>
        public static readonly RedisHelper Instance = new RedisHelper();

        /// <summary>
        /// 获取真实键名称
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static string GetKey(string key)
        {
            return key;
        }

        /// <summary>
        /// 获取真实键名称
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static string GetKey(UserInfo userInfo, string key)
        {
            return $"{userInfo.UserId}:{key}";
        }
    }
}
