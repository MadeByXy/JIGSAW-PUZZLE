using RegistryLibrary.Attribute;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;
using System;

namespace RegistryLibrary.Interface.Action
{
    /// <summary>
    /// 指示该对象存在公开的创建方法
    /// </summary>
    /// <typeparam name="DataType">模块类型</typeparam>
    public interface ICreateAction<DataType>
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="data">模块对象信息</param>
        /// <param name="userInfo">创建人</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被创建时抛出</exception>
        /// <returns>创建结果对象</returns>
        DataType Create(DataType data, UserInfo userInfo);

        /// <summary>
        /// 创建人
        /// </summary>
        UserInfo Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateDate { get; set; }
    }
}
