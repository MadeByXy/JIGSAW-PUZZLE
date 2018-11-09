using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Dependence;

namespace MessageQueueModule
{
    /// <summary>
    /// 注册模块
    /// </summary>
    public class InjectionModule : IInjection
    {
        public InjectionModule(ILogging log)
        {
            Log = log;
        }

        /// <summary>
        /// 日志模块
        /// </summary>
        public static ILogging Log { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public const string ModuleName = "消息队列模块";
    }
}
