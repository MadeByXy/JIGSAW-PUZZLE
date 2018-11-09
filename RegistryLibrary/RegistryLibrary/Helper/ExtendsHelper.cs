using Newtonsoft.Json;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// 扩展方法帮助类
    /// </summary>
    public static class ExtendsHelper
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">待转换的类型</typeparam>
        /// <param name="originalObject">原始数据</param>
        /// <returns></returns>
        public static T ToType<T>(this object originalObject)
        {
            if (originalObject == null)
            {
                return default(T);
            }
            else if (originalObject.GetType() == typeof(string))
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)originalObject;
                }
                else
                {

                    return JsonConvert.DeserializeObject<T>(originalObject as string);
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)JsonConvert.SerializeObject(originalObject);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(originalObject));
                }
            }
        }
    }
}
