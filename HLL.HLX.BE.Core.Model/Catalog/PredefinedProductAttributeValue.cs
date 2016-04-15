using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a predefined (default) product attribute value
    /// </summary>
    public class PredefinedProductAttributeValue : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the product attribute identifier
        /// </summary>
        public int ProductAttributeId { get; set; }

        /// <summary>
        ///     Gets or sets the product attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the price adjustment
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        ///     Gets or sets the weight adjustment
        /// </summary>
        public decimal WeightAdjustment { get; set; }

        /// <summary>
        ///     Gets or sets the attibute value cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///     Gets the product attribute
        /// </summary>
        public virtual ProductAttribute ProductAttribute { get; set; }
    }
}