namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    public interface IGroupInfo : IOrganizInfo
    {
        /// <summary>
        /// 用户组Id
        /// </summary>
        int GroupId { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        string GroupName { get; set; }
    }
}
