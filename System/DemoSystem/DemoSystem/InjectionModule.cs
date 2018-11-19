using DemoModule.Model;
using LoggingModule.Model;
using MessageQueueModule.Model;
using RegistryLibrary.AppModule;
using RegistryLibrary.BasicModule;
using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Dependence;
using System;

namespace DemoSystem
{
    public class InjectionModule : IInjection
    {
        public InjectionModule()
        {
            var log = new Logging();

            new MessageQueueModule.InjectionModule(log);
            var messageQueue = new MessageQueue();

            new DemoModule.InjectionModule(messageQueue);
            DemoModule = new Demo();

            new WebSocketModule.InjectionModule(messageQueue);
            WebSocketModule = new WebSocketModule.Model.WebSocket(4141);

            DemoModule.PrepareDeleteEvent.Subscribe((DemoModel d, UserInfo u) =>
            {
                Console.WriteLine("错误注入测试成功");
                return new ApiResult<DBNull, DBNull> { Success = false, Message = "错误注入测试" };
            });

        }

        /// <summary>
        /// Demo模块
        /// </summary>
        public static IDemo DemoModule { get; set; }

        /// <summary>
        /// WebSocket模块
        /// </summary>
        public static IWebSocket WebSocketModule { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public const string ModuleName = "Demo系统";
    }
}
