using RegistryLibrary.Interface.Common;
using System.Collections.Generic;

namespace RegistryLibrary.Interface.Action
{
    /// <summary>
    /// 指示该对象存在公开的批量删除方法
    /// </summary>
    /// <typeparam name="KeyType">主键类型</typeparam>
    public interface IBatchDeleteAction<KeyType>
    {
        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="primaryKeyList">对象主键列表</param>
        /// <param name="userInfo">删除人信息</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被删除时抛出</exception>
        void BatchDelete(List<KeyType> primaryKeyList, UserInfo userInfo);
    }
}
