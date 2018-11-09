namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public interface IRoleInfo
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        string RoleName { get; set; }
    }
}
