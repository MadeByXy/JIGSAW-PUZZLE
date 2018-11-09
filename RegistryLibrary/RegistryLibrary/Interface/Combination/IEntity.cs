using RegistryLibrary.Interface.Action;
using RegistryLibrary.Interface.Event;

namespace RegistryLibrary.Interface.Combination
{
    /// <summary>
    /// 通用实体类型
    /// 增删改查
    /// </summary>
    /// <typeparam name="DataType">实体类型</typeparam>
    /// <typeparam name="KeyType">主键类型</typeparam>
    public interface IEntity<DataType, KeyType> :
        ICreateEvent<DataType>, ICreateAction<DataType>,
        IModifiedEvent<DataType>, IModifiedAction<DataType>,
        IDeleteEvent<DataType>, IDeleteAction<KeyType>, IBatchDeleteAction<KeyType>,
        IDetailAction<DataType, KeyType>
    { }
}
