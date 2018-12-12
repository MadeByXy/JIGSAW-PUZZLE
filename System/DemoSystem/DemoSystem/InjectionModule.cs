using RegistryLibrary.AppModule;
using RegistryLibrary.BasicModule;

namespace DemoSystem
{
    public class InjectionModule
    {
        /// <summary>
        /// Demo模块
        /// </summary>
        public static IDemo DemoModule { get; set; }

        /// <summary>
        /// Email模块
        /// </summary>
        public static ICommunicating EmailModule { get; set; }

        /// <summary>
        /// WebSocket模块
        /// </summary>
        public static IWebSocket WebSocketModule { get; set; }

        /// <summary>
        /// 定时服务模块
        /// </summary>
        public static ITimingService TimingServiceModule { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public const string ModuleName = "Demo系统";
    }
}
