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
        /// 发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        public void Cancel(Guid sendKey)
        {
            InjectionModule.TimingService.Cancel(sendKey);
        }

        /// <summary>
        /// 发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        public async void Send(CommunicatingModel data)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(InjectionModule.Sender),
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
                using (var email = new SmtpClient(InjectionModule.SenderHost))
                {
                    email.Credentials = new NetworkCredential(InjectionModule.Sender.Split('@')[0], InjectionModule.Password);
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
            return InjectionModule.TimingService.Invoke(() => Send(data), sendDate);
        }
    }
}
