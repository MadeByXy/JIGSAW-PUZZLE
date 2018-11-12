using RegistryLibrary.Event;
using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.Interface.Event
{
    /// <summary>
    /// 指示该对象在创建期间会发出事件
    /// </summary>
    /// <typeparam name="DataType">模块对象</typeparam>
    public interface ICreateEvent<DataType>
    {
        /// <summary>
        /// 创建前发出的事件
        /// 指示是否可以被创建
        /// </summary>
        MessageEvent<DataType, UserInfo> PrepareCreatedEvent { get; set; }

        /// <summary>
        /// 创建后发出的事件
        /// </summary>
        MessageEvent<DataType, UserInfo> CreatedEvent { get; set; }
    }
}
