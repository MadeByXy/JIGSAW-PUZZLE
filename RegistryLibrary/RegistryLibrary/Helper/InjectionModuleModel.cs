using System.Collections.Generic;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// 注入模块实体
    /// </summary>
    public class InjectionModuleModel
    {
        /// <summary>
        /// 模块名称
        /// 该值为空表示不被其他模块使用
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 待注入构造参数集合
        /// </summary>
        /// <remarks>
        ///     1. 当类为静态类时该属性无效
        ///     2. 当类存在多个实例方法时将自动指定最匹配的一个
        /// </remarks>
        public List<InjectionParameterModel> ConstructorList { get; set; } = new List<InjectionParameterModel>();

        /// <summary>
        /// 待注入属性集合
        /// </summary>
        /// <remarks>
        ///     1. 静态属性或实例属性均可
        /// </remarks>
        public List<InjectionParameterModel> PropertyList { get; set; } = new List<InjectionParameterModel>();
    }
}
