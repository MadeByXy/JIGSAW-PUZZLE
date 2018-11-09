using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using System;

namespace FileModule.Model
{
    /// <summary>
    /// 上传文件实体
    /// 事件模块
    /// </summary>
    public partial class UploadFile
    {
        /// <summary>
        /// 创建后发出的事件
        /// </summary>
        public event Action<IUploadFile, IUserInfo> CreatedEvent;

        /// <summary>
        /// 删除后发出的事件
        /// </summary>
        public event Action<IUploadFile, IUserInfo> DeleteEvent;

        /// <summary>
        /// 创建前发出的事件
        /// 指示是否可以被创建
        /// </summary>
        public event Func<IUploadFile, IUserInfo, IResult> PrepareCreatedEvent;

        /// <summary>
        /// 删除前发出的事件
        /// 指示是否可以被删除
        /// </summary>
        public event Func<IUploadFile, IUserInfo, IResult> PrepareDeleteEvent;
    }
}
