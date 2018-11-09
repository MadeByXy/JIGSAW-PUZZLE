namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    public class GroupInfo : IOrganizInfo
    {
        /// <summary>
        /// 用户组Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        public string OrganizId { get; set; }

        /// <summary>
        /// 组织机构ID对应数据表
        /// </summary>
        public string OrganizTable { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizName { get; set; }
    }
}
