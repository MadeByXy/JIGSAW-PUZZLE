using System;
using System.Collections.Generic;

namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class UserInfo : IUserInfo
    {
        public string Account { get; set; }

        public bool IsAdmin { get; set; }

        public Guid OpenId { get; set; }

        public string OrganizId { get; set; }

        public string OrganizName { get; set; }

        public string OrganizTable { get; set; }

        public List<PositionInfo> PositonsList { get; set; } = new List<PositionInfo>();

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
