using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Dependence;

namespace WebSocketModule
{
    /// <summary>
    /// 注册模块
    /// </summary>
    /// <remarks>引用本项目的同时, 还应在System项目下下引用SuperWebSocket包, 否则会造成启动失败</remarks>
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
