using RegistryLibrary.Interface.Common;

namespace RegistryLibrary.Interface.Action
{
    /// <summary>
    /// 指示该对象存在公开的详情查询方法
    /// </summary>
    /// <typeparam name="DataType">模块类型</typeparam>
    /// <typeparam name="KeyType">主键类型</typeparam>
    public interface IDetailAction<DataType, KeyType>
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="primaryKey">对象主键</param>
        /// <param name="userInfo">查看人信息</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被查看时抛出</exception>
        DataType Detail(KeyType primaryKey, UserInfo userInfo);
    }
}
