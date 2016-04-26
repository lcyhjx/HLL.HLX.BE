using System;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a back in stock subscription
    /// </summary>
    public class BackInStockSubscription : FullAuditedEntity<int, User>
    {
        /// <summary>
        ///     Gets or sets the store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the customer identifier
        /// </summary>
        public long  CustomerId { get; set; }

        /// <summary>
        ///     Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///     Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        ///     Gets the customer
        /// </summary>
        public virtual User Customer { get; set; }
    }
}