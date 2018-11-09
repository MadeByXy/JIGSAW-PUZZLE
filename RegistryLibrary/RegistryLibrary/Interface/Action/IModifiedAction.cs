using RegistryLibrary.Interface.Common;
using System;

namespace RegistryLibrary.Interface.Action
{
    /// <summary>
    /// 指示该对象存在公开的修改方法
    /// </summary>
    /// <typeparam name="DataType">模块对象</typeparam>
    public interface IModifiedAction<DataType>
    {
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="data">模块对象信息</param>
        /// <param name="userInfo">修改人</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被修改时抛出</exception>
        /// <returns>修改结果</returns>
        DataType Modified(DataType data, UserInfo userInfo);

        /// <summary>
        /// 最后一次修改人
        /// </summary>
        UserInfo Modifier { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        DateTime ModifiedDate { get; set; }
    }
}
