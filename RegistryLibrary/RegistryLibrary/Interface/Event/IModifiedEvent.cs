using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.Interface.Event
{
    /// <summary>
    /// 指示该对象在修改期间会发出事件
    /// </summary>
    /// <typeparam name="DataType">模块对象</typeparam>
    public interface IModifiedEvent<DataType>
    {
        /// <summary>
        /// 修改前发出的事件
        /// 指示是否可以被修改
        /// </summary>
        MessageEvent<DataType, UserInfo> PrepareModifiedEvent { get; set; }

        /// <summary>
        /// 修改后发出的事件
        /// </summary>
        MessageEvent<DataType, UserInfo> ModifiedEvent { get; set; }
    }
}
