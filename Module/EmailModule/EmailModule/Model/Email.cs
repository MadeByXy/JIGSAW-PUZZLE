using RegistryLibrary.BasicModule;
using RegistryLibrary.Helper;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailModule.Model
{
    public class Email : ICommunicating
    {
        /// <summary>
        /// 实例化邮箱模块
        /// </summary>
        /// <param name="sender">邮件发送人</param>
        /// <param name="password">发送人密码</param>
        /// <param name="host">发送人所在stmp服务host</param>
        /// <param name="timingService">定时服务模块实例</param>
        public Email(string sender, string password, string host, ITimingService timingService)
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

        /// <summary>
        /// 发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        public void Cancel(Guid sendKey)
        {
            TimingService.Cancel(sendKey);
        }

        /// <summary>
        /// 发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        public async void Send(CommunicatingModel data)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(Sender),
                Subject = data.Title,
                Body = data.Content
            };

            foreach (var recipient in data.Recipients)
            {
                message.To.Add(new MailAddress(recipient));
            }

            if (data.Extra.ContainsKey("cc"))
            {
                foreach (var cc in data.Extra["cc"].ToType<string[]>())
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            if (data.Extra.ContainsKey("bcc"))
            {
                foreach (var bcc in data.Extra["bcc"].ToType<string[]>())
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }

            if (data.Extra.ContainsKey("isBodyHtml"))
            {
                message.IsBodyHtml = bool.Parse(data.Extra["isBodyHtml"]);
            }

            await Task.Run(() =>
            {
                using (var email = new SmtpClient(SenderHost))
                {
                    email.Credentials = new NetworkCredential(Sender.Split('@')[0], Password);
                    email.Send(message);
                }
            });
        }

        /// <summary>
        /// 延时发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        /// <param name="sendDate">发送时间</param>
        /// <returns>用以取消发送的Key</returns>
        public Guid SendAsync(CommunicatingModel data, DateTime sendDate)
        {
            return TimingService.Invoke(() => Send(data), sendDate);
        }
    }
}
