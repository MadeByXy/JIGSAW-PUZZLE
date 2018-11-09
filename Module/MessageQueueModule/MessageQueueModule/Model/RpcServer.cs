using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageQueueModule.Model
{
    public class RpcServer : SimpleRpcServer
    {
        public RpcServer(Subscription subscription) : base(subscription) { }

        private static Dictionary<string, List<Func<byte[], Result>>> Consumers { get; set; } = new Dictionary<string, List<Func<byte[], Result>>>();

        public void AddConsumers(string queueName, Func<byte[], Result> action)
        {
            lock (Consumers)
            {
                if (!Consumers.ContainsKey(queueName))
                {
                    Consumers.Add(queueName, new List<Func<byte[], Result>>());
                }
            }
            Consumers[queueName].Add(action);
        }

        public override byte[] HandleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            replyProperties = null;

            //通过队列名称获取Callback方法
            var queueName = Encoding.UTF8.GetString((byte[])requestProperties.Headers["queueName"]);
            if (Consumers.ContainsKey(queueName) && Consumers[queueName].Count > 0)
            {
                foreach (var action in Consumers[queueName])
                {
                    var result = action(body);
                    if (!result.Success)
                    {
                        return result.ToSerialization();
                    }
                }
            }
            return new Result { Success = true }.ToSerialization();
        }
    }
}
