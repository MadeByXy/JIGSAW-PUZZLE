using RegistryLibrary.AppModule;
using RegistryLibrary.Exception;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;

namespace DemoSystem
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    var message = MessageQueueModule.Model.MessageQueueModule.Instance;

        //    const string queueName = "hello";
        //    message.QueueDeclare(queueName);

        //    message.Subscribe(queueName, new Func<string, bool>(data =>
        //    {
        //        Console.WriteLine("成功接收消息");
        //        Console.WriteLine($"消息内容: {data}");
        //        return true;
        //    }));

        //    message.Publish(queueName, "生产者成功发送消息");

        //    Console.Read();
        //}

        static void Main(string[] args)
        {
            new InjectionModule();

            Console.WriteLine("启动成功");

            var demo = InjectionModule.DemoModule;
            var userInfo = new UserInfo { UserId = "test", UserName = "test_user" };
            var data = new DemoModel { PrimaryKey = 10, Message = "系统调用验证" };

            Invoke(demo, userInfo, data);

            //var nowDate = DateTime.Now;
            //var loop = 1;
            //for (var i = 0; i < loop; i++)
            //{
            //    Invoke(demo, userInfo, data);
            //}
            //var span = DateTime.Now - nowDate;
            //Console.WriteLine($"执行{loop}次, 共计用时: {span.TotalSeconds}秒, 平均用时: {(span.TotalMilliseconds / loop)}毫秒");
            Console.Read();
        }
        static void Invoke(IDemo demo, UserInfo userInfo, DemoModel data)
        {
            try
            {
                userInfo.UserName += "_c";
                demo.Create(data, userInfo);
            }
            catch (ActionForbiddenException e)
            {
                Console.WriteLine($"创建执行失败：{(e.InnerException ?? e).Message}");
            }

            try
            {
                userInfo.UserName += "_e";
                data.Message = "修改调用验证";
                demo.Modified(data, userInfo);
            }
            catch (ActionForbiddenException e)
            {
                Console.WriteLine($"修改执行失败：{(e.InnerException ?? e).Message}");
            }

            try
            {
                userInfo.UserName += "_d";
                demo.Delete(data.PrimaryKey, userInfo);
            }
            catch (ActionForbiddenException e)
            {
                Console.WriteLine($"删除执行失败：{(e.InnerException ?? e).Message}");
            }
        }
    }
}
