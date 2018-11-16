namespace RegistryLibrary.ImplementsClass
{
    /// <summary>
    /// 传输类型枚举
    /// </summary>
    public enum WebSocketDataEnum
    {
        /// <summary>
        /// 提交数据并等待回复
        /// </summary>
        Submit = 0,

        /// <summary>
        /// 发布事件
        /// </summary>
        Publish = 1,

        /// <summary>
        /// 订阅频道
        /// </summary>
        Subscribe = 2,
    }
}
