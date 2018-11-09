namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 机构信息
    /// </summary>
    public class OrganizInfo : IOrganizInfo
    {
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
