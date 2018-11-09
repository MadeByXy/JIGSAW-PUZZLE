namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 日志模块
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="e">异常信息</param>
        /// <param name="extraData">额外数据</param>
        void LogException(string moduleName, System.Exception e, string extraData = "");

        /// <summary>
        /// 日常记录
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="info">记录信息</param>
        void LogInfo(string moduleName, string info);
    }
}
