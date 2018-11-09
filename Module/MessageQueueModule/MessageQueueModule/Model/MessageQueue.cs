using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using RabbitMQ.Client.MessagePatterns;
using RegistryLibrary.BasicModule;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageQueueModule.Model
{
    public class MessageQueue : IMessageQueue
    {
        private static readonly MessageQueue Instance = new MessageQueue("");

        /// <summary>
        /// 初始化<see cref="MessageQueue"/>
        /// </summary>
        /// <param name="data"></param>
        private MessageQueue(string data)
        {
            var factory = new ConnectionFactory();
            factory.UserName = ConnectionFactory.DefaultUser;
            factory.Password = ConnectionFactory.DefaultPass;
            factory.VirtualHost = ConnectionFactory.DefaultVHost;
            factory.HostName = "localhost";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            Connection = factory.CreateConnection();
        }

        /// <summary>
        /// 实例化<see cref="MessageQueue"/>
        /// </summary>
        public MessageQueue()
        {
            Connection = Instance.Connection;
            Channel = Connection.CreateModel();
            Channel.ContinuationTimeout = new TimeSpan(0, 0, 1);

            const string queueName = "rpc_channel";
            QueueDeclare(queueName);
            RpcServer = new RpcServer(new Subscription(Channel, queueName));
            RpcClient = new SimpleRpcClient(Channel, new PublicationAddress(
                exchangeType: ExchangeType.Direct,
                exchangeName: "",
                routingKey: queueName));

            //MainLoop方法会阻塞线程, 所以要放到Task中
            Task.Run(() => { RpcServer.MainLoop(); });
        }

        private IConnection Connection { get; set; }

        /// <summary>
        /// 系统唯一标识
        /// </summary>
        private static string SystemId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// RabbitMQ 连接频道
        /// </summary>
        private IModel Channel { get; set; }

        /// <summary>
        /// RabbitMQ RPC发布服务
        /// </summary>
        private RpcServer RpcServer { get; set; }

        /// <summary>
        /// RabbitMQ RPC接收服务
        /// </summary>
        private SimpleRpcClient RpcClient { get; set; }

        /// <summary>
        /// 消费者列表
        /// </summary>
        private static Dictionary<string, EventingBasicConsumer> Consumers { get; set; } = new Dictionary<string, EventingBasicConsumer>();

        /// <summary>
        /// 声明队列
        /// </summary>
        /// <param name="queueName">队列名称</param>
        public void QueueDeclare(string queueName)
        {
            Channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        public void Publish<T>(string queueName, T data)
        {
            Publish(queueName, data, "");
        }

        /// <summary>
        /// 生成Callback队列名称
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <returns></returns>
        private string CreateCallbackName(string queueName)
        {
            return $"{queueName}_Callback";
        }

        /// <summary>
        /// 发布消息到指定目标
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <param name="exchange">目标位置</param>
        public void Publish<T>(string queueName, T data, string exchange)
        {
            QueueDeclare(queueName);
            Channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: data.ToSerialization());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <param name="callback">接收订阅者的回复</param>
        public void Publish<T>(string queueName, T data, Action<Result> callback)
        {
            var result = RpcClient.Call(new BasicProperties
            {
                Headers = new Dictionary<string, object>
                {
                    { nameof(queueName), queueName }
                }
            }, data.ToSerialization());
            callback(result.Body.FromSerialization<Result>());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息主体</param>
        /// <returns>订阅者的回复结果</returns>
        public async Task<List<Result>> PublishAsync<T>(string queueName, T data)
        {
            return await Task.Run(() =>
            {
                var resultList = new List<Result>();
                Publish(queueName, data, new Action<Result>((result) =>
                {
                    resultList.Add(result);
                }));

                return resultList;
            });
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        public void Subscribe<T>(string queueName, Func<T, Result> callback)
        {
            RpcServer.AddConsumers(queueName, (bytes) =>
            {
                var data = bytes.FromSerialization<T>();
                return callback?.Invoke(data) ?? new Result { Success = true };
            });
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">消息数据类型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        public void Subscribe<T>(string queueName, Action<T> callback)
        {
            ConfirmConsumer(queueName);
            Consumers[queueName].Received += (model, deliver) =>
            {
                try
                {
                    var data = deliver.Body.FromSerialization<T>();
                    callback?.Invoke(data);
                }
                catch (Exception e)
                {
                    InjectionModule.Log.LogException(InjectionModule.ModuleName, e, $"队列名称: {queueName}");
                }
            };
        }

        /// <summary>
        /// 确认消费者
        /// </summary>
        /// <param name="queueName">队列名称</param>
        private void ConfirmConsumer(string queueName)
        {
            lock (Consumers)
            {
                if (!Consumers.ContainsKey(queueName))
                {
                    QueueDeclare(queueName);
                    EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);

                    consumer.Received += (model, deliver) =>
                    {
                        //标记为已读
                        Channel.BasicAck(deliver.DeliveryTag, false);
                    };

                    //指定消费队列
                    Channel.BasicConsume(
                        queue: queueName,
                        autoAck: false,
                        consumer: consumer);

                    Consumers.Add(queueName, consumer);
                }
            }
        }
    }
}
