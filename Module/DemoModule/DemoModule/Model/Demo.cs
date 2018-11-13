﻿using RegistryLibrary.AppModule;
using RegistryLibrary.Event;
using RegistryLibrary.Exception;
using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoModule.Model
{
    /// <summary>
    /// Demo模块
    /// </summary>
    public partial class Demo : IDemo
    {
        public Demo()
        {
            PrepareCreatedEvent = new InternalEvent<DemoModel, UserInfo>();
            //PrepareCreatedEvent = new MessageEvent<DemoModel, UserInfo>("Demo.PrepareCreatedEvent", InjectionModule.MessageQueue);
            CreatedEvent = new MessageEvent<DemoModel, UserInfo>("Demo.CreatedEvent", InjectionModule.MessageQueue);

            PrepareModifiedEvent = new InternalEvent<DemoModel, UserInfo>(); 
            //PrepareModifiedEvent = new MessageEvent<DemoModel, UserInfo>("Demo.PrepareModifiedEvent", InjectionModule.MessageQueue);
            ModifiedEvent = new MessageEvent<DemoModel, UserInfo>("Demo.ModifiedEvent", InjectionModule.MessageQueue);

            PrepareDeleteEvent = new InternalEvent<DemoModel, UserInfo>();
            //PrepareDeleteEvent = new MessageEvent<DemoModel, UserInfo>("Demo.PrepareDeleteEvent", InjectionModule.MessageQueue);
            DeleteEvent = new MessageEvent<DemoModel, UserInfo>("Demo.DeleteEvent", InjectionModule.MessageQueue);

            //注入消息队列
            PrepareCreatedEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"预创建验证监听失败测试, 创建人：{userInfo.UserName}, 附加消息:{data.Message}");
                return new ApiResult<DBNull, DBNull> { Success = true, Message = "测试通过" };
            }).Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"预创建验证监听成功2, 创建人：{userInfo.UserName}, 附加消息:{data.Message}");
                return new ApiResult<DBNull, DBNull> { Success = true, Message = "测试通过" };
            });

            CreatedEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"创建结束监听成功, 创建人：{userInfo.UserName}, 附加消息:{data.Message}");
            }).Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"创建结束监听成功1, 创建人：{userInfo.UserName}, 附加消息:{data.Message}");
            });

            PrepareModifiedEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"预修改验证监听成功, 修改人：{userInfo.UserName}, 附加消息:{data.Message}");
                return new ApiResult<DBNull, DBNull> { Success = true, Message = "测试通过" };
            });

            ModifiedEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"修改结束监听成功, 修改人：{userInfo.UserName}, 附加消息:{data.Message}");
            });

            PrepareDeleteEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"预删除验证监听成功, 删除人：{userInfo.UserName}, 附加消息:{data.Message}");
                return new ApiResult<DBNull, DBNull> { Success = true, Message = "测试不通过测试" };
            });

            DeleteEvent.Subscribe((DemoModel data, UserInfo userInfo) =>
            {
                Console.WriteLine($"删除结束监听成功, 删除人：{userInfo.UserName}, 附加消息:{data.Message}");
            });
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public int PrimaryKey { get; set; }

        public string Message { get; set; }

        public DateTime CreateDate { get; set; }

        public UserInfo Creator { get; set; }

        public DateTime DeleteDate { get; set; }

        public UserInfo DeletingPerson { get; set; }

        public bool IsDelete { get; set; }

        public DateTime ModifiedDate { get; set; }

        public UserInfo Modifier { get; set; }

        public void BatchDelete(List<int> primaryKeyList, UserInfo userInfo)
        {
            var taskList = new List<Task>();
            primaryKeyList.ForEach(primaryKey =>
            {
                taskList.Add(Task.Run(() =>
                {
                    Delete(primaryKey, userInfo);
                }).ContinueWith((t) =>
                {
                    if (t.Exception != null)
                    {
                        //Todo: 删除失败, 记录异常
                    }
                }));
            });
        }

        public DemoModel Create(DemoModel data, UserInfo userInfo)
        {
            var result = PrepareCreatedEvent?.PublishAsync(data, userInfo).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }

            Console.WriteLine("创建成功");

            CreatedEvent?.Publish(data, userInfo);
            return data;
        }

        public void Delete(int primaryKey, UserInfo userInfo)
        {
            var data = Detail(primaryKey, userInfo);
            var result = PrepareDeleteEvent?.PublishAsync(data, userInfo).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }

            Console.WriteLine("删除成功");

            DeleteEvent?.Publish(data, userInfo);
        }

        public DemoModel Detail(int primaryKey, UserInfo userInfo)
        {
            return new DemoModel() { PrimaryKey = 1, Message = "详情" };
        }

        public DemoModel Modified(DemoModel data, UserInfo userInfo)
        {
            var result = PrepareModifiedEvent?.PublishAsync(data, userInfo).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }

            Console.WriteLine("修改成功");

            ModifiedEvent?.Publish(data, userInfo);
            return data;
        }
    }
}
