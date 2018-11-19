using Newtonsoft.Json;
using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using SuperWebSocket;
using System;
using System.Collections.Generic;

namespace WebSocketModule.Model
{
    /// <summary>
    /// WebSocket功能实现模块
    /// </summary>
    public class WebSocket : IWebSocket
    {
        /// <summary>
        /// <see cref="WebSocket"/>的唯一实例
        /// </summary>
        public static readonly WebSocket Instance = new WebSocket();

        /// <summary>
        /// 初始化<see cref="WebSocket"/>
        /// </summary>
        private WebSocket()
        {
            Socket = new WebSocketServer();

            Socket.NewSessionConnected += Socket_NewSessionConnected;
            Socket.NewMessageReceived += Socket_NewMessageReceived;
            Socket.SessionClosed += Socket_SessionClosed;
            Socket.Setup(4142);
            Socket.Start();

            Initialization();
        }

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

        /// <summary>
        /// 用户在cookie中的身份标识所在位置
        /// </summary>
        private const string UserName = "_userinfo_userId";

        #region 内部方法
        /// <summary>
        /// 数据初始化
        /// </summary>
        private void Initialization()
        {
            ActionTable.Add(WebSocketTypeEnum.Publish, (data) =>
            {
                InjectionModule.MessageQueue.Publish(data.Channel, data.Data);
            });

            ActionTable.Add(WebSocketTypeEnum.Subscribe, (data) =>
            {
                InjectionModule.MessageQueue.Subscribe<object>(data.Channel, (result) =>
                {
                    Publish(data.UserInfo, data.Channel, result);
                });
            });

            ActionTable.Add(WebSocketTypeEnum.Submit, (data) =>
            {
                Publish(data.UserInfo, data.Channel, InjectionModule.MessageQueue.PublishAsync(data.Channel, data.Data).Result);
            });
        }

        /// <summary>
        /// 加入连接
        /// </summary>
        /// <param name="session">连接信息</param>
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

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="session">连接信息</param>
        /// <param name="value">接收值</param>
        private void Socket_NewMessageReceived(WebSocketSession session, string value)
        {
            var data = JsonConvert.DeserializeObject<WebSocketModel<object>>(value);

            //根据不同的类型执行不同的操作
            ActionTable[data.Type].Invoke(data);
        }

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
        #endregion

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
                    session.Send(JsonConvert.SerializeObject(new WebSocketModel<T>
                    {
                        Type = WebSocketTypeEnum.Publish,
                        Channel = channel,
                        Data = data
                    }));
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
                    session.Send(JsonConvert.SerializeObject(new WebSocketModel<T>
                    {
                        Type = WebSocketTypeEnum.Publish,
                        Channel = channel,
                        Data = data
                    }));
                }
            }
        }
    }
}
