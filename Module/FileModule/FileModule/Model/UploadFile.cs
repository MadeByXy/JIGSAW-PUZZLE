using RegistryLibrary.BasicModule;
using RegistryLibrary.Interface.Common;
using System;

namespace FileModule.Model
{
    /// <summary>
    /// 上传文件实体
    /// </summary>
    public partial class UploadFile : IUploadFile
    {
        /// <summary>
        /// <see cref="UploadFile"/>的唯一实例
        /// </summary>
        public static readonly UploadFile Instance = new UploadFile();

        /// <summary>
        /// <see cref="UploadFile"/>的实例方法
        /// </summary>
        private UploadFile() { }

        /// <summary>
        /// 文件Id
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// 文件名称（带扩展名）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        public double FileSize { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileSuffix { get; set; }

        /// <summary>
        /// 指示对象是否已被删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 文件所在物理路径
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public IUserInfo Creator { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteDate { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public IUserInfo DeletingPerson { get; set; }

        /// <summary>
        /// 文件来源模块
        /// </summary>
        public string FromModule { get; set; }

        /// <summary>
        /// 文件所属机构
        /// </summary>
        public IOrganizInfo FromOrganiz { get; set; }
    }
}
