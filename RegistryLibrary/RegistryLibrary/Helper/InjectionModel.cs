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
}
