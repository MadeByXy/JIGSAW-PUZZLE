using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.Interface.Event
{
    /// <summary>
    /// 指示该对象在删除期间会发出事件
    /// </summary>
    /// <typeparam name="DataType">模块对象</typeparam>
    public interface IDeleteEvent<DataType>
    {
        /// <summary>
        /// 删除前发出的事件
        /// 指示是否可以被删除
        /// </summary>
        IEvent<DataType, UserInfo> PrepareDeleteEvent { get; set; }

        /// <summary>
        /// 删除后发出的事件
        /// </summary>
        IEvent<DataType, UserInfo> DeleteEvent { get; set; }
    }
}
