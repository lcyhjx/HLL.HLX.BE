using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Shipping;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a record to manage product inventory per warehouse
    /// </summary>
    public class ProductWarehouseInventory : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the warehouse identifier
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        ///     Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        ///     Gets or sets the reserved quantity (ordered but not shipped yet)
        /// </summary>
        public int ReservedQuantity { get; set; }

        /// <summary>
        ///     Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        ///     Gets the warehouse
        /// </summary>
        public virtual Warehouse Warehouse { get; set; }
    }
}