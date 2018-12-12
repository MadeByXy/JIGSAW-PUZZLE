using System.Collections.Generic;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// 注入实体
    /// </summary>
    public class InjectionModel
    {
        /// <summary>
        /// 实例集合
        /// </summary>
        public Dictionary<string, object> InstanceCollection { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 注入模块实体集合
        /// </summary>
        public List<InjectionModuleModel> Modules { get; set; } = new List<InjectionModuleModel>();
    }

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

    /// <summary>
    /// 注入参数实体
    /// </summary>
    public class InjectionParameterModel
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数依赖情况
        /// 该值为空表示该参数不依赖注入
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// 参数注入值
        /// 当<see cref="Ref"/>值为空时该值才生效，会对指定项注入固定数据
        /// </summary>
        public string Value { get; set; }
    }
}
