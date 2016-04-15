using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a product review helpfulness
    /// </summary>
    public class ProductReviewHelpfulness : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the product review identifier
        /// </summary>
        public int ProductReviewId { get; set; }

        /// <summary>
        ///     A value indicating whether a review a helpful
        /// </summary>
        public bool WasHelpful { get; set; }

        /// <summary>
        ///     Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        ///     Gets the product
        /// </summary>
        public virtual ProductReview ProductReview { get; set; }
    }
}