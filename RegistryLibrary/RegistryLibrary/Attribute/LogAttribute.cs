using PostSharp.Aspects;
using System;

namespace RegistryLibrary.Attribute
{
    /// <summary>
    /// 指示该对象要进行异常日志记录
    /// </summary>
    [Serializable]
    public class LogExceptionAttribute : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"{args.Method.Name}方法执行异常：{(args.Exception.InnerException ?? args.Exception).Message}");
        }
    }

    /// <summary>
    /// 指示该对象要进行耗时记录
    /// </summary>
    [Serializable]
    public class TimeSpanRecordAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 指示方法的进入时间
        /// </summary>
        private DateTime EnterDate { get; set; }

        public override void OnEntry(MethodExecutionArgs args)
        {
            EnterDate = DateTime.Now;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"{args.Method.Name}方法共用耗时：{(DateTime.Now - EnterDate).TotalMilliseconds}毫秒");
        }
    }
}
