using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// WebSocket模块
    /// </summary>
    public interface IWebSocket
    {
        /// <summary>
        /// 发送消息给指定频道
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        void Publish<T>(string channel, T data);

        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        void Publish<T>(UserInfo userInfo, string channel, T data);
    }
}
