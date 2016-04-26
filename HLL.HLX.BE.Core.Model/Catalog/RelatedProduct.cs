using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a related product
    /// </summary>
    public class RelatedProduct : FullAuditedEntity<int, User>
    {
        /// <summary>
        ///     Gets or sets the first product identifier
        /// </summary>
        public int ProductId1 { get; set; }

        /// <summary>
        ///     Gets or sets the second product identifier
        /// </summary>
        public int ProductId2 { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}