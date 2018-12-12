namespace RegistryLibrary.Helper
{
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
