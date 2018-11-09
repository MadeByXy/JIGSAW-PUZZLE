namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 带成功标识的结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 指示是否操作成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 指示附加信息
        /// </summary>
        public string Message { get; set; }
    }
}
