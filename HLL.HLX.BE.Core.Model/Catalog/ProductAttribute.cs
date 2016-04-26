using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a product attribute
    /// </summary>
    public class ProductAttribute : FullAuditedEntity<int,User>
    {
        /// <summary>
        ///     Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description
        /// </summary>
        public string Description { get; set; }
    }
}