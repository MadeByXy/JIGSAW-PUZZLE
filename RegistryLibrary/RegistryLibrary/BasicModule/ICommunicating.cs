using System;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 通信模块
    /// </summary>
    public interface ICommunicating
    {
        /// <summary>
        /// 发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        void Send(CommunicatingModel data);

        /// <summary>
        /// 延时发起通信
        /// </summary>
        /// <param name="data">通信内容</param>
        /// <param name="sendDate">发送时间</param>
        /// <returns>用以取消发送的Key</returns>
        Guid SendAsync(CommunicatingModel data, DateTime sendDate);

        /// <summary>
        /// 取消发送通信
        /// </summary>
        /// <param name="sendKey">通信Key</param>
        void Cancel(Guid sendKey);
    }
}
