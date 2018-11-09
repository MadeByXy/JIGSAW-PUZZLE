using RegistryLibrary.BasicModule;
using System;

namespace LoggingModule.Model
{
    /// <summary>
    /// 日志模块
    /// </summary>
    public class Logging : ILogging
    {
        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="e">异常信息</param>
        /// <param name="extraData">额外数据</param>
        public void LogException(string moduleName, Exception e, string extraData = "")
        {
            Console.WriteLine($"日志记录(异常)：{moduleName}, {(e.InnerException ?? e).Message}");
        }

        /// <summary>
        /// 日常记录
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="info">记录信息</param>
        public void LogInfo(string moduleName, string info)
        {
            throw new NotImplementedException();
        }
    }
}
