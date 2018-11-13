using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// 数据序列化帮助类
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public static byte[] ToSerialization<T>(this T data)
        {
            return Encoding.UTF8.GetBytes(JToken.FromObject(data).ToString());
        }

        /// <summary>
        /// 数据反序列化
        /// </summary>
        /// <typeparam name="T">需要的数据类型</typeparam>
        /// <param name="bytes">序列化数据</param>
        /// <returns></returns>
        public static T FromSerialization<T>(this byte[] bytes)
        {
            switch (typeof(T).Name)
            {
                case nameof(String):
                    return (T)(object)Encoding.UTF8.GetString(bytes);
                default:
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
            }
        }
    }
}
