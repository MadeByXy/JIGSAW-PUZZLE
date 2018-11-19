using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// WebSocket交互用数据结构
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class WebSocketModel<T>
    {
        /// <summary>
        /// 本次会话的唯一标识
        /// 该值由前台自动生成, 后台仅作返回, 不进行其他逻辑处理
        /// </summary>
        public string Seed { get; set; }

        /// <summary>
        /// 指示本次会话的用户名称
        /// </summary>
        public UserInfo UserInfo { get; set; } = new UserInfo { UserId = "admin" };

        /// <summary>
        /// 访问类型
        /// </summary>
        public WebSocketTypeEnum Type { get; set; }

        /// <summary>
        /// 发送频道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 传递数据
        /// </summary>
        public T Data { get; set; }
    }
}
