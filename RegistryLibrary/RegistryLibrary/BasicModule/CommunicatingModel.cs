using RegistryLibrary.Interface.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 通信内容数据实体
    /// </summary>
    public class CommunicatingModel
    {
        /// <summary>
        /// 接收人列表
        /// </summary>
        public List<string> Recipients { get; set; } = new List<string>();

        /// <summary>
        /// 发送人
        /// </summary>
        public UserInfo Sender { get; set; }

        /// <summary>
        /// 发送标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发送模板
        /// </summary>
        /// <example>【XX网络】亲爱的${xm}, 您的验证码为${code}, ${time}内有效。</example>
        public string Template { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public Dictionary<string, string> TemplateContent { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content
        {
            get
            {
                var content = Template;
                foreach (Match match in Regex.Matches(content, "\\${([a-zA-Z]*)}"))
                {
                    string data;
                    if (TemplateContent.TryGetValue(match.Groups[1].Value, out data))
                    {
                        content = content.Replace(match.Groups[0].Value, data);
                    }
                }
                return content;
            }
        }

        /// <summary>
        /// 附加信息, 做预留用
        /// </summary>
        public Dictionary<string, string> Extra { get; set; } = new Dictionary<string, string>();
    }
}
