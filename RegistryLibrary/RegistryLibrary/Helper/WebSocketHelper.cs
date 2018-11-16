using Newtonsoft.Json;
using RegistryLibrary.BasicModule;
using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;
using SuperWebSocket;
using System.Collections.Generic;

namespace RegistryLibrary.Helper
{
    public class WebSocketHelper
    {
        /// <summary>
        /// <see cref="WebSocketHelper"/>的唯一实例
        /// </summary>
        public static readonly WebSocketHelper Instance = new WebSocketHelper();

        /// <summary>
        /// 连接池
        /// 指示用户与连接的关系
        /// </summary>
        private static Dictionary<string, List<WebSocketSession>> ConnectionPool { get; set; } = new Dictionary<string, List<WebSocketSession>>();

        /// <summary>
        /// 映射表
        /// 指示Seed与Session的连接关系
        /// </summary>
        private static Dictionary<string, WebSocketSession> MappingTable { get; set; } = new Dictionary<string, WebSocketSession>();

        /// <summary>
        /// WebSocket连接对象
        /// </summary>
        private static WebSocketServer Socket { get; set; }

        /// <summary>
        /// 用户在cookie中的身份标识所在位置
        /// </summary>
        private const string UserName = "_userinfo_userId";

        private WebSocketHelper()
        {
            Socket = new WebSocketServer();

            Socket.NewSessionConnected += Socket_NewSessionConnected;
            Socket.NewMessageReceived += Socket_NewMessageReceived;
            Socket.SessionClosed += Socket_SessionClosed;
            Socket.Setup(4142);
            Socket.Start();
        }

        private IMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void Socket_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            //关闭连接
            var userName = session.Cookies[UserName];
            if (ConnectionPool.ContainsKey(userName))
            {
                ConnectionPool[userName].Remove(session);
            }
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void Socket_NewMessageReceived(WebSocketSession session, string value)
        {
            var data = JsonConvert.DeserializeObject<WebSocketData>(value);

            //接收到消息后转发给指定频道
            MessageQueue.PublishAsync(data.Channel, data);
        }

        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        public void SendMessage<T>(UserInfo userInfo, string channel, T data)
        {
            if (ConnectionPool.ContainsKey(userInfo.UserId))
            {
                foreach (var session in ConnectionPool[userInfo.UserId])
                {
                    session.Send(JsonConvert.SerializeObject(new WebSocketData
                    {
                        Type = WebSocketDataEnum.Publish,
                        Channel = channel,
                        Data = data
                    }));
                }
            }
        }

        /// <summary>
        /// 发送消息给指定频道
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        public void SendMessage<T>(string channel, T data)
        {

        }

        /// <summary>
        /// 加入连接
        /// </summary>
        /// <param name="session"></param>
        private void Socket_NewSessionConnected(WebSocketSession session)
        {
            var userName = session.Cookies[UserName];
            lock (ConnectionPool)
            {
                if (!ConnectionPool.ContainsKey(userName))
                {
                    ConnectionPool.Add(userName, new List<WebSocketSession>());
                }
            }
            ConnectionPool[userName].Add(session);
        }
    }
}
