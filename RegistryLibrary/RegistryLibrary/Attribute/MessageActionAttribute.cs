using PostSharp.Aspects;
using RegistryLibrary.BasicModule;
using RegistryLibrary.Exception;
using RegistryLibrary.Interface.Action;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryLibrary.Attribute
{
    /// <summary>
    /// 指示对象会在创建执行前发起验证请求，并在结束后推送结果数据
    /// </summary>
    [Serializable]
    public class CreateActionAttribute : OnMethodBoundaryAspect
    {
        private const string PrepareEventName = nameof(ICreateEvent<DBNull>.PrepareCreatedEvent);

        private const string EventName = nameof(ICreateEvent<DBNull>.CreatedEvent);

        public override void OnEntry(MethodExecutionArgs args)
        {
            //等待返回结果, 如果动作被禁止, 不允许进入方法
            var prepareEvt = args.Instance.GetType().GetProperty(PrepareEventName).GetValue(args.Instance);
            var method = prepareEvt.GetType().GetMethod(nameof(IMessageQueue.PublishAsync));
            var result = ((Task<Result>)method.Invoke(prepareEvt, args.Arguments.Select(item => item).ToArray())).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            //结束后推送结果数据, 第一个返回值固定推结果
            var evt = args.Instance.GetType().GetProperty(EventName).GetValue(args.Instance);
            var method = evt.GetType().GetMethod(nameof(IMessageQueue.Publish));
            method.Invoke(evt, args.Arguments.Select((item, index) => index == 0 ? args.ReturnValue : item).ToArray());
        }
    }

    /// <summary>
    /// 指示对象会在修改执行前发起验证请求，并在结束后推送结果数据
    /// </summary>
    [Serializable]
    public class ModifiedActionAttribute : OnMethodBoundaryAspect
    {
        private const string PrepareEventName = nameof(IModifiedEvent<DBNull>.PrepareModifiedEvent);

        private const string EventName = nameof(IModifiedEvent<DBNull>.ModifiedEvent);

        public override void OnEntry(MethodExecutionArgs args)
        {
            //等待返回结果, 如果动作被禁止, 不允许进入方法
            var prepareEvt = args.Instance.GetType().GetProperty(PrepareEventName).GetValue(args.Instance);
            var method = prepareEvt.GetType().GetMethod(nameof(IMessageQueue.PublishAsync));
            var result = ((Task<Result>)method.Invoke(prepareEvt, args.Arguments.Select(item => item).ToArray())).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            //结束后推送结果数据, 第一个返回值固定推结果
            var evt = args.Instance.GetType().GetProperty(EventName).GetValue(args.Instance);
            var method = evt.GetType().GetMethod(nameof(IMessageQueue.Publish));
            method.Invoke(evt, args.Arguments.Select((item, index) => index == 0 ? args.ReturnValue : item).ToArray());
        }
    }

    /// <summary>
    /// 指示对象会在删除执行前发起验证请求，并在结束后推送结果数据
    /// </summary>
    [Serializable]
    public class DeleteActionAttribute : OnMethodBoundaryAspect
    {
        private const string PrepareEventName = nameof(IDeleteEvent<DBNull>.PrepareDeleteEvent);

        private const string EventName = nameof(IDeleteEvent<DBNull>.DeleteEvent);

        /// <summary>
        /// 本次的详情结果
        /// </summary>
        private object Result { get; set; }

        public override void OnEntry(MethodExecutionArgs args)
        {
            //等待返回结果, 如果动作被禁止, 不允许进入方法
            var prepareEvt = args.Instance.GetType().GetProperty(PrepareEventName).GetValue(args.Instance);
            var method = prepareEvt.GetType().GetMethod(nameof(IMessageQueue.PublishAsync));

            //要先通过id换取详情
            Result = args.Instance.GetType().GetMethod(nameof(IDetailAction<DBNull, DBNull>.Detail)).Invoke(
               args.Instance, args.Arguments.Select(item => item).ToArray());

            //第一个值固定传详情
            var result = ((Task<Result>)method.Invoke(prepareEvt,
                args.Arguments.Select((item, index) => index == 0 ? Result : item).ToArray())).Result;
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            //结束后推送结果数据, 第一个返回值固定推详情
            var evt = args.Instance.GetType().GetProperty(EventName).GetValue(args.Instance);
            var method = evt.GetType().GetMethod(nameof(IMessageQueue.Publish));
            method.Invoke(evt, args.Arguments.Select((item, index) => index == 0 ? Result : item).ToArray());
        }
    }
}
