using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a tier price
    /// </summary>
    public class TierPrice : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the store identifier (0 - all stores)
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        ///     Gets or sets the customer role identifier
        /// </summary>
        public int? CustomerRoleId { get; set; }

        /// <summary>
        ///     Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets the product
        /// </summary>
        public virtual Product Product { get; set; }

        ///// <summary>
        /////     Gets or sets the customer role
        ///// </summary>
        //public virtual CustomerRole CustomerRole { get; set; }
    }
}