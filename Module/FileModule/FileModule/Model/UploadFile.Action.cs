using DysoftLib;
using RegistryLibrary.BasicModule;
using RegistryLibrary.Exception;
using RegistryLibrary.Helper;
using RegistryLibrary.Interface.Common;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Threading.Tasks;

namespace FileModule.Model
{
    /// <summary>
    /// 上传文件实体
    /// 功能模块
    /// </summary>
    public partial class UploadFile
    {
        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="fileIdList">附件Id列表</param>
        /// <param name="userInfo">删除人信息</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被删除时抛出</exception>
        public void BatchDelete(List<int> fileIdList, IUserInfo userInfo)
        {
            var taskList = new List<Task>();
            fileIdList.ForEach(fileId =>
            {
                taskList.Add(Task.Run(() =>
                {
                    Delete(fileId, userInfo);
                }).ContinueWith((t) =>
                {
                    if (t.Exception != null)
                    {
                        //Todo: 删除失败, 记录异常
                    }
                }));
            });
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="data">模块对象信息</param>
        /// <param name="userInfo">创建人</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被创建时抛出</exception>
        public void Create(IUploadFile data, IUserInfo userInfo)
        {
            var result = PrepareCreatedEvent?.Invoke(data, userInfo);
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }

            OracleDB.ExeSql(
                @"insert into UploadFiles
                    (fileId,
                     fileName,
                     fileSize,
                     fileSuffix,
                     localPath,
                     fromModule,
                     creator,
                     creatorName,
                     organizId,
                     organizTable,
                     organizName)
                  values
                    (:fileId,
                     :fileName,
                     :fileSize,
                     :fileSuffix,
                     :localPath,
                     :fromModule,
                     :creator,
                     :creatorName,
                     :organizId,
                     :organizTable,
                     :organizName)",
                new OracleParameter("fileId", data.FileId),
                new OracleParameter("fileName", data.FileName),
                new OracleParameter("fileSize", data.FileSize),
                new OracleParameter("fileSuffix", data.FileSuffix),
                new OracleParameter("localPath", data.LocalPath),
                new OracleParameter("fromModule", data.FromModule),
                new OracleParameter("creator", userInfo.UserId),
                new OracleParameter("creatorName", userInfo.UserName),
                new OracleParameter("organizId", userInfo.OrganizId),
                new OracleParameter("organizTable", userInfo.OrganizTable),
                new OracleParameter("organizName", userInfo.OrganizName));

            CreatedEvent?.Invoke(data, userInfo);
        }

        /// <summary>
        /// 数据库环境检测
        /// </summary>
        public void DBEnvironmentCheck()
        {
            DataTableHelper.VerificationTable("UploadFiles", "fileId");
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="fileId">附件Id</param>
        /// <param name="userInfo">删除人信息</param>
        /// <exception cref="Exception.ActionForbiddenException">当不允许被删除时抛出</exception>
        public void Delete(int fileId, IUserInfo userInfo)
        {
            var data = Detail(fileId, userInfo);
            var result = PrepareDeleteEvent?.Invoke(data, userInfo);
            if (!(result?.Success ?? true))
            {
                throw new ActionForbiddenException(result.Message);
            }

            //删除时标记为删除, 真实文件的删除由垃圾文件清理机制执行。
            OracleDB.ExeSql(
                @"update UploadFiles
                     set isDelete       = 0,
                         deleteDate     = to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
                         deletingPerson = :deletingPerson,
                         deletingName   = :deletingName
                   where fileId = :fileId",
                new OracleParameter("deletingPerson", userInfo.UserId),
                new OracleParameter("deletingName", userInfo.UserName),
                new OracleParameter("fileId", fileId));

            DeleteEvent?.Invoke(data, userInfo);
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="fileId">附件Id</param>
        /// <param name="userInfo">查看人信息</param>
        public IUploadFile Detail(int fileId, IUserInfo userInfo)
        {
            var data = OracleDB.GetDataTable(
                @"select to_char(fileId) fileId,
                         fileName,
                         fileSize,
                         fileSuffix,
                         localPath,
                         fromModule,
                         createDate,
                         deleteDate
                    from UploadFiles
                   where fileId = :fileId
                     and isDelete = 0",
                new OracleParameter("fileId", fileId)).ToObject<IUploadFile>();
            if (data.FileId == 0)
            {
                throw new NullReferenceException("附件不存在，可能已被删除。");
            }

            //加载创建人信息
            data.Creator = OracleDB.GetDataTable(
                "select creator, creatorName from UploadFiles where fileId = :fileId",
                new OracleParameter("fileId", fileId)).ToObject<IUserInfo>();

            //加载机构信息
            data.FromOrganiz = OracleDB.GetDataTable(
                @"select to_char(organizId) organizId, organizTable, organizName
                    from UploadFiles
                   where fileId = :fileId",
                new OracleParameter("fileId", fileId)).ToObject<IOrganizInfo>();

            return data;
        }
    }
}
