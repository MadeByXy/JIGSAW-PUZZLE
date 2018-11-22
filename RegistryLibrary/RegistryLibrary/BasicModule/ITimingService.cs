using System;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 定时服务模块
    /// </summary>
    public interface ITimingService
    {
        /// <summary>
        /// 开启定时服务
        /// </summary>
        /// <param name="action">到时间后要执行的动作</param>
        /// <param name="date">执行时间</param>
        /// <returns>执行Key, 用以执行后续动作</returns>
        Guid Invoke(Action action, DateTime date);

        /// <summary>
        /// 开启定时服务
        /// </summary>
        /// <param name="action">到时间后要执行的动作</param>
        /// <param name="span">距离目前的时间段</param>
        /// <returns>执行Key, 用以执行后续动作</returns>
        Guid Invoke(Action action, TimeSpan span);

        /// <summary>
        /// 取消动作
        /// </summary>
        /// <param name="invokeKey">执行Key</param>
        void Cancel(Guid invokeKey);

        /// <summary>
        /// 查询动作的剩余时间
        /// </summary>
        /// <param name="invokeKey">执行Key</param>
        /// <returns>距离执行的剩余时间</returns>
        TimeSpan QueryRemainingTime(Guid invokeKey);
    }
}
