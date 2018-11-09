namespace RegistryLibrary.Exception
{
    /// <summary>
    /// 该异常指示动作被禁止
    /// </summary>
    public class ActionForbiddenException : System.Exception
    {
        /// <summary>
        /// 实例化异常类
        /// </summary>
        /// <param name="message">被禁止的原因</param>
        public ActionForbiddenException(string message) : base(message) { }
    }
}
