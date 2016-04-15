using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a category template
    /// </summary>
    public class CategoryTemplate : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the template name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the view path
        /// </summary>
        public string ViewPath { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}