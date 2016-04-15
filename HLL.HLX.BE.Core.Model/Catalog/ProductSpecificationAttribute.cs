using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a product specification attribute
    /// </summary>
    public class ProductSpecificationAttribute : FullAuditedEntity<long, User>
    {
        /// <summary>
        ///     Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the attribute type ID
        /// </summary>
        public int AttributeTypeId { get; set; }

        /// <summary>
        ///     Gets or sets the specification attribute identifier
        /// </summary>
        public int SpecificationAttributeOptionId { get; set; }

        /// <summary>
        ///     Gets or sets the custom value
        /// </summary>
        public string CustomValue { get; set; }

        /// <summary>
        ///     Gets or sets whether the attribute can be filtered by
        /// </summary>
        public bool AllowFiltering { get; set; }

        /// <summary>
        ///     Gets or sets whether the attribute will be shown on the product page
        /// </summary>
        public bool ShowOnProductPage { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///     Gets or sets the product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        ///     Gets or sets the specification attribute option
        /// </summary>
        public virtual SpecificationAttributeOption SpecificationAttributeOption { get; set; }

        /// <summary>
        ///     Gets the attribute control type
        /// </summary>
        public SpecificationAttributeType AttributeType
        {
            get { return (SpecificationAttributeType) AttributeTypeId; }
            set { AttributeTypeId = (int) value; }
        }
    }
}