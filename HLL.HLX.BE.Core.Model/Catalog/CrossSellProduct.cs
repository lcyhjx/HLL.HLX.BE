using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a cross-sell product
    /// </summary>
    public class CrossSellProduct : FullAuditedEntity<int, User>
    {
        /// <summary>
        ///     Gets or sets the first product identifier
        /// </summary>
        public long ProductId1 { get; set; }

        /// <summary>
        ///     Gets or sets the second product identifier
        /// </summary>
        public long ProductId2 { get; set; }
    }
}