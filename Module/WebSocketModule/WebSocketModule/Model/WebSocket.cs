using Newtonsoft.Json;
using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSocketModule.Model
{
    /// <summary>
    /// WebSocket功能实现模块
    /// </summary>
    public class WebSocket : IWebSocket
    {
        /// <summary>
        /// 初始化<see cref="WebSocket"/>
        /// </summary>
        /// <param name="messageQueue">消息队列模块</param>
        /// <param name="port">WebSocket监听端口号</param>
        public WebSocket(IMessageQueue messageQueue, int port)
        {
            MessageQueue = messageQueue;

            Socket = new WebSocketServer();

            Socket.NewSessionConnected += SessionConnected;
            Socket.NewMessageReceived += MessageReceived;
            Socket.SessionClosed += SessionClosed;
            Socket.Setup(port);
            Socket.Start();

            Initialization();
        }

        /// <summary>
        /// 消息队列模块
        /// </summary>
        private IMessageQueue MessageQueue { get; set; }

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
        /// 枚举功能列表
        /// </summary>
        private static Dictionary<WebSocketTypeEnum, Action<WebSocketModel<object>>> ActionTable { get; set; } = new Dictionary<WebSocketTypeEnum, Action<WebSocketModel<object>>>();

        /// <summary>
        /// WebSocket连接对象
        /// </summary>
        private static WebSocketServer Socket { get; set; }

        #region 内部方法
        /// <summary>
        /// 数据初始化
        /// </summary>
        private void Initialization()
        {
            ActionTable.Add(WebSocketTypeEnum.Publish, (data) =>
            {
                Task.Run(() =>
                {
                    MessageQueue.Publish(data.Channel, data.Data);
                });
            });

            ActionTable.Add(WebSocketTypeEnum.Subscribe, (data) =>
            {
                Task.Run(() =>
                {
                    MessageQueue.Subscribe<object>(data.Channel, (result) =>
                    {
                        Publish(data.UserInfo, data.Channel, result);
                    });
                });
            });

            ActionTable.Add(WebSocketTypeEnum.Submit, (data) =>
            {
                Task.Run(() =>
                {
                    Publish(data.UserInfo, data.Channel, MessageQueue.PublishAsync(data.Channel, data.Data).Result);
                });
            });
        }

        /// <summary>
        /// 根据连接session获取用户标识
        /// </summary>
        /// <param name="session">连接信息</param>
        /// <returns></returns>
        private static UserInfo GetUserInfo(WebSocketSession session)
        {
            return new UserInfo { UserId = "admin" };
        }

        /// <summary>
        /// 加入连接
        /// </summary>
        /// <param name="session">连接信息</param>
        private void SessionConnected(WebSocketSession session)
        {
            var userId = GetUserInfo(session).UserId;
            lock (ConnectionPool)
            {
                if (!ConnectionPool.ContainsKey(userId))
                {
                    ConnectionPool.Add(userId, new List<WebSocketSession>());
                }
            }
            ConnectionPool[userId].Add(session);
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="session">连接信息</param>
        /// <param name="value">接收值</param>
        private void MessageReceived(WebSocketSession session, string value)
        {
            var data = JsonConvert.DeserializeObject<WebSocketModel<object>>(value);
            data.UserInfo = GetUserInfo(session);

            //根据不同的类型执行不同的操作
            ActionTable[data.Type].Invoke(data);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="session">连接信息</param>
        /// <param name="reason">关闭原因</param>
        private void SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason reason)
        {
            //关闭连接
            var userName = GetUserInfo(session).UserId;
            if (ConnectionPool.ContainsKey(userName))
            {
                ConnectionPool[userName].Remove(session);
            }
        }
        #endregion

        /// <summary>
        /// 发送消息给指定连接
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="session">连接信息</param>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        public void Publish<T>(WebSocketSession session, string channel, T data)
        {
            session.TrySend(JsonConvert.SerializeObject(new WebSocketModel<T>
            {
                Type = WebSocketTypeEnum.Publish,
                Channel = channel,
                Data = data
            }));
        }

        /// <summary>
        /// 发送消息给指定频道
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        public void Publish<T>(string channel, T data)
        {
            foreach (var connects in ConnectionPool)
            {
                foreach (var session in connects.Value)
                {
                    Publish(session, channel, data);
                }
            }
        }

        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="userInfo">用户信息</param>
        /// <param name="channel">推送频道</param>
        /// <param name="data">推送数据</param>
        public void Publish<T>(UserInfo userInfo, string channel, T data)
        {
            if (ConnectionPool.ContainsKey(userInfo.UserId))
            {
                foreach (var session in ConnectionPool[userInfo.UserId])
                {
                    Publish(session, channel, data);
                }
            }
        }
    }
}
