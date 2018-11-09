using RegistryLibrary.Interface.Action;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Dependence;
using RegistryLibrary.Interface.Event;

namespace RegistryLibrary.BasicModule
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public interface IUploadFile :
        ICreateEvent<IUploadFile>, ICreateAction<IUploadFile>,
        IDeleteEvent<IUploadFile>, IDeleteAction<int>, IBatchDeleteAction<int>,
        IDetailAction<IUploadFile, int>,
        IDatabase
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        int FileId { get; set; }

        /// <summary>
        /// 文件名称（带扩展名）
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        double FileSize { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        string FileSuffix { get; set; }

        /// <summary>
        /// 文件所在物理路径
        /// </summary>
        string LocalPath { get; set; }

        /// <summary>
        /// 文件来源模块
        /// </summary>
        string FromModule { get; set; }

        /// <summary>
        /// 文件所属机构
        /// </summary>
        IOrganizInfo FromOrganiz { get; set; }
    }
}
