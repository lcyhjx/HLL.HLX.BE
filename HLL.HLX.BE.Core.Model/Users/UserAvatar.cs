using Abp.Domain.Entities.Auditing;

namespace HLL.HLX.BE.Core.Model.Users
{
    /// <summary>
    ///     用户头像实体
    /// </summary>
    public class UserAvatar : FullAuditedEntity<int, User>
    {
        /// <summary>
        ///     用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///     图片存储路径
        /// </summary>
        public string ImageFilePath { get; set; }

        /// <summary>
        ///     图片名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }
    }
}