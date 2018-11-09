namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 机构信息
    /// </summary>
    public interface IOrganizInfo
    {
        /// <summary>
        /// 组织机构ID
        /// </summary>
        string OrganizId { get; set; }

        /// <summary>
        /// 组织机构ID对应数据表
        /// </summary>
        string OrganizTable { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        string OrganizName { get; set; }
    }
}
