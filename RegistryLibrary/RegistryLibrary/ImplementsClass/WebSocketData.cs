namespace RegistryLibrary.ImplementsClass
{
    /// <summary>
    /// WebSocket数据通信实体
    /// </summary>
    public class WebSocketData
    {
        /// <summary>
        /// 本次会话的唯一标识
        /// 该值由前台自动生成, 后台仅作返回, 不进行其他逻辑处理
        /// </summary>
        public string Seed { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public WebSocketDataEnum Type { get; set; }

        /// <summary>
        /// 发送频道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 传递数据
        /// </summary>
        public object Data { get; set; }
    }
}
