using DemoModule.Model;
using LoggingModule.Model;
using MessageQueueModule.Model;
using RegistryLibrary.AppModule;
using RegistryLibrary.BasicModule;
using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Dependence;
using System;
using TimingServiceModule.Model;

namespace DemoSystem
{
    public class InjectionModule : IInjection
    {
        public InjectionModule()
        {
            //初始化日志模块
            var log = new Logging();

            //初始化消息队列模块
            new MessageQueueModule.InjectionModule(log);
            var messageQueue = new MessageQueue();

            //初始化WebSocket模块
            new WebSocketModule.InjectionModule(messageQueue);
            WebSocketModule = new WebSocketModule.Model.WebSocket(4141);

            //初始化WebApi模块
            new WebApiModule.InjectionModule("http://localhost:4142");

            //初始化定时服务模块
            var timingService = new TimingService();

            //初始化Email模块
            new EmailModule.InjectionModule("xy609284278@126.com", "xy10742581xy", "smtp.126.com", timingService);
            //new EmailModule.InjectionModule("609284278@qq.com", "oljkxlpsihvobdjg", "smtp.qq.com", timingService);
            EmailModule = new EmailModule.Model.Email();

            //初始化demo模块
            new DemoModule.InjectionModule(messageQueue);
            DemoModule = new Demo();

            //注入事件
            DemoModule.PrepareDeleteEvent.Subscribe((DemoModel model, UserInfo user) =>
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
        /// Email模块
        /// </summary>
        public static ICommunicating EmailModule { get; set; }

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
