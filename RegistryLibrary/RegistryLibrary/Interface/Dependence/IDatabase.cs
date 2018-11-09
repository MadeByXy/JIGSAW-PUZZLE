namespace RegistryLibrary.Interface.Dependence
{
    /// <summary>
    /// 指示该对象依赖于数据库
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// 数据库环境检测
        /// </summary>
        void DBEnvironmentCheck();
    }
}
