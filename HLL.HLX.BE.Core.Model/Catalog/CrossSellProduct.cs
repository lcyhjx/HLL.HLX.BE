using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a cross-sell product
    /// </summary>
    public class CrossSellProduct : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the first product identifier
        /// </summary>
        public int ProductId1 { get; set; }

        /// <summary>
        ///     Gets or sets the second product identifier
        /// </summary>
        public int ProductId2 { get; set; }
    }
}