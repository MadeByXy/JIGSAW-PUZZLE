using RegistryLibrary.Interface.Common;
using System.Collections.Generic;

namespace RegistryLibrary.ImplementsClass
{
    /// <summary>
    /// Api通用返回结果
    /// </summary>
    /// <typeparam name="T">obj类型</typeparam>
    /// <typeparam name="V">rows类型</typeparam>
    public class ApiResult<T, V> : Result
    {
        /// <summary>
        /// 单项数据返回
        /// </summary>
        public T Object { get; set; }

        /// <summary>
        /// 多项数据返回
        /// </summary>
        public IEnumerable<V> Rows { get; set; }

        /// <summary>
        /// 数据数量
        /// </summary>
        public int Total { get; set; } = 0;
    }
}
