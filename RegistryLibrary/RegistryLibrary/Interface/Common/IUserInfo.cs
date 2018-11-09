using System;
using System.Collections.Generic;

namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public interface IUserInfo : IOrganizInfo, IRoleInfo
    {
        /// <summary>
        /// 用户唯一识别标志
        /// </summary>
        Guid OpenId { get; set; }

        /// <summary>
        /// 用户中心主键Id
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 人员账号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        bool IsAdmin { get; set; }

        /// <summary>
        /// 人员所属岗位
        /// </summary>
        List<PositionInfo> PositonsList { get; set; }
    }
}
