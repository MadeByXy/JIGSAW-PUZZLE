using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimingServiceModule.Model
{
    /// <summary>
    /// 定时服务模块数据实体
    /// </summary>
    public class TimingServiceModel
    {
        /// <summary>
        /// 定时Id
        /// </summary>
        public Guid TimingId { get; set; }

        /// <summary>
        /// 要执行的时间
        /// </summary>
        public DateTime InvokeDate { get; set; }

        /// <summary>
        /// 剩余时间
        /// </summary>
        public TimeSpan InvokeSpan
        {
            get
            {
                return InvokeDate - DateTime.Now;
            }
        }

        /// <summary>
        /// 结束后要执行的动作
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// 用于取消定时
        /// </summary>
        public CancellationTokenSource Source { get; set; }
    }
}
