using RegistryLibrary.Interface.Common;
using System;

namespace RegistryLibrary.Interface.Action
{
    /// <summary>
    /// 指示该对象存在公开的删除方法
    /// </summary>
    /// <typeparam name="KeyType">主键类型</typeparam>
    public interface IDeleteAction<KeyType>
    {
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="primaryKey">对象主键</param>
        /// <param name="userInfo">删除人信息</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被删除时抛出</exception>
        void Delete(KeyType primaryKey, UserInfo userInfo);
        
        /// <summary>
        /// 指示对象是否已被删除
        /// </summary>
        bool IsDelete { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        UserInfo DeletingPerson { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime DeleteDate { get; set; }
    }
}
