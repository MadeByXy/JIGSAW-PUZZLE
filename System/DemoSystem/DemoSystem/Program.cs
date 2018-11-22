using RegistryLibrary.AppModule;
using RegistryLibrary.Exception;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;
using RegistryLibrary.Attribute;

[assembly: LogException]
namespace DemoSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            new InjectionModule();

            Console.WriteLine("启动成功");

            var email = InjectionModule.EmailModule;
            var timing = InjectionModule.TimingServiceModule;
            var demo = InjectionModule.DemoModule;
            var userInfo = new UserInfo { UserId = "test", UserName = "test_user" };
            var data = new DemoModel { PrimaryKey = 10, Message = "系统调用验证" };

            for (var i = 1; i <= 10; i++)
            {
                var time = i;
                timing.Invoke(() => Console.WriteLine($"{time}秒后执行"), TimeSpan.FromSeconds(time));
            }

            //email.Send(new RegistryLibrary.BasicModule.CommunicatingModel()
            //{
            //    Template = "测试发送：${code}",
            //    TemplateContent = new System.Collections.Generic.Dictionary<string, string>
            //    {
            //        { "code", "123456" }
            //    },
            //    Recipients = new System.Collections.Generic.List<string> {
            //        "xy609284278@126.com",
            //        "609284278@qq.com",
            //    }
            //});

            Invoke(demo, userInfo, data);

            RedisHelper.Set("测试", new UserInfo { UserId = "测试", UserName = "测试" });
            RedisHelper.Set(userInfo, "测试", "文字测试");

            Console.WriteLine(RedisHelper.Get<string>(userInfo, "测试").Result);
            Console.WriteLine(RedisHelper.Get<UserInfo>("测试").Result.UserId);

            var channel = "test";
            RedisHelper.Subscribe(channel, (string value) =>
            {
                Console.WriteLine(value);
            });

            RedisHelper.Subscribe(channel, (string value) =>
            {
                Console.WriteLine($"第二个订阅者：{value}");
            });

            RedisHelper.Publish(channel, "我通过redis发布了一条数据");
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

        [TimeSpanRecord]
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
