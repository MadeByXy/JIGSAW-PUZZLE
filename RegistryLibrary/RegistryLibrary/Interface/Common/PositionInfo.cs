namespace RegistryLibrary.Interface.Common
{
    /// <summary>
    /// 用户岗位信息
    /// </summary>
    public class PositionInfo : IPositionInfo
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string OrganizId { get; set; }

        public string OrganizName { get; set; }

        public string OrganizTable { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }
    }
}
