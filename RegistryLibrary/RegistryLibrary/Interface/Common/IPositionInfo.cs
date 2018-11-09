namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 用户岗位信息
    /// </summary>
    public interface IPositionInfo : IGroupInfo
    {
        /// <summary>
        /// 岗位Id
        /// </summary>
        int PositionId { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        string PositionName { get; set; }
    }
}
