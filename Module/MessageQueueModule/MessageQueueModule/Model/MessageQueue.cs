using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RegistryLibrary.BasicModule;
using System.Threading.Tasks;

namespace MessageQueueModule.Model
{
    /// <summary>
    /// 基于RabbitMQ的消息队列模块
    /// </summary>
    public partial class MessageQueue : IMessageQueue
    {
        private static readonly MessageQueue Instance = new MessageQueue();

        /// <summary>
        /// 初始化<see cref="MessageQueue"/>
        /// </summary>
        private MessageQueue()
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
        /// <param name="logging">日志模块</param>
        /// </summary>
        public MessageQueue(ILogging logging)
        {
            Logging = logging;
            Connection = Instance.Connection;
            Channel = Connection.CreateModel();

            const string queueName = "rpc_channel";
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            RpcServer = new RpcServer(new Subscription(Channel, queueName));
            RpcClient = new SimpleRpcClient(Channel, new PublicationAddress(
                exchangeType: ExchangeType.Direct,
                exchangeName: ExchangeName,
                routingKey: queueName));

            //MainLoop方法会阻塞线程, 所以要放到Task中
            Task.Run(() => { RpcServer.MainLoop(); });
        }

        /// <summary>
        /// 日志模块
        /// </summary>
        private ILogging Logging { get; set; }

        private IConnection Connection { get; set; }

        private const string ExchangeName = "amq.direct";

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
    }
}
