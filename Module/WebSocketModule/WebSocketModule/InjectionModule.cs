using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Dependence;

namespace WebSocketModule
{
    /// <summary>
    /// 注册模块
    /// </summary>
    public class InjectionModule : IInjection
    {
        public InjectionModule(IMessageQueue messageQueue)
        {
            MessageQueue = messageQueue;
        }

        /// <summary>
        /// 消息队列模块
        /// </summary>
        public static IMessageQueue MessageQueue { get; private set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public const string ModuleName = "WebSocket模块";
    }
}
