using RegistryLibrary.BasicModule;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TimingServiceModule.Model
{
    /// <summary>
    /// 定时服务模块
    /// </summary>
    public class TimingService : ITimingService
    {
        private Dictionary<Guid, TimingServiceModel> TimingDic = new Dictionary<Guid, TimingServiceModel>();

        /// <summary>
        /// 取消动作
        /// </summary>
        /// <param name="invokeKey">执行Key</param>
        public void Cancel(Guid invokeKey)
        {
            TimingServiceModel model;
            if (TimingDic.TryGetValue(invokeKey, out model))
            {
                model.Source.Cancel();
            }
        }

        /// <summary>
        /// 开启定时服务
        /// </summary>
        /// <param name="action">到时间后要执行的动作</param>
        /// <param name="span">距离目前的时间段</param>
        /// <returns>执行Key, 用以执行后续动作</returns>
        public Guid Invoke(Action action, TimeSpan span)
        {
            return Invoke(action, DateTime.Now + span);
        }

        /// <summary>
        /// 开启定时服务
        /// </summary>
        /// <param name="action">到时间后要执行的动作</param>
        /// <param name="date">执行时间</param>
        /// <returns>执行Key, 用以执行后续动作</returns>
        public Guid Invoke(Action action, DateTime date)
        {
            var model = new TimingServiceModel
            {
                TimingId = Guid.NewGuid(),
                Source = new CancellationTokenSource(),
                Action = action,
                InvokeDate = date
            };

            TimingDic.Add(model.TimingId, model);

            Task.Delay(model.InvokeSpan, model.Source.Token).ContinueWith(t =>
            {
                if (t.Exception == null)
                {
                    TimingDic.Remove(model.TimingId);
                    action();
                }
            });

            return model.TimingId;
        }

        /// <summary>
        /// 查询动作的剩余时间
        /// </summary>
        /// <param name="invokeKey">执行Key</param>
        /// <returns>距离执行的剩余时间</returns>
        public TimeSpan QueryRemainingTime(Guid invokeKey)
        {
            TimingServiceModel model;
            if (TimingDic.TryGetValue(invokeKey, out model))
            {
                return model.InvokeSpan;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
    }
}
