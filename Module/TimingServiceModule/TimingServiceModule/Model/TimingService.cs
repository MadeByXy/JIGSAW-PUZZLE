using RegistryLibrary.BasicModule;
using System;

namespace TimingServiceModule.Model
{
    /// <summary>
    /// 定时服务模块
    /// </summary>
    public class TimingService : ITimingService
    {
        public void Cancel(int invokeKey)
        {
            throw new NotImplementedException();
        }

        public int Invoke(Action action, DateTime date)
        {
            throw new NotImplementedException();
        }

        public DateTime QueryRemainingTime(int invokeKey)
        {
            throw new NotImplementedException();
        }
    }
}
