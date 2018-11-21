using EmailModule.Model;
using RegistryLibrary.BasicModule;

namespace EmailModule
{
    /// <summary>
    /// 注入模块
    /// </summary>
    public class InjectionModule
    {
        /// <summary>
        /// 实例化邮箱模块
        /// </summary>
        /// <param name="sender">邮件发送人</param>
        /// <param name="password">发送人密码</param>
        /// <param name="host">发送人所在stmp服务host</param>
        /// <param name="timingService">定时服务模块实例</param>
        public InjectionModule(string sender, string password, string host, ITimingService timingService)
        {
            Sender = sender;
            Password = password;
            SenderHost = host;
            TimingService = timingService;
        }

        /// <summary>
        /// 邮件发送人
        /// </summary>
        public static string Sender { get; private set; }

        /// <summary>
        /// 邮件发送人密码
        /// </summary>
        internal static string Password { get; private set; }

        /// <summary>
        /// 邮件发送人Host
        /// </summary>
        public static string SenderHost { get; private set; }

        /// <summary>
        /// 定时服务模块实例
        /// </summary>
        public static ITimingService TimingService { get; private set; }

        public const string ModuleName = "Email发送模块";
    }
}
