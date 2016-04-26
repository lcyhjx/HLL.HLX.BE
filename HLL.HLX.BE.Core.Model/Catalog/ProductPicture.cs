using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Media;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a product picture mapping
    /// </summary>
    public class ProductPicture : FullAuditedEntity<int, User>
    {
        /// <summary>
        ///     Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///     Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        ///     Gets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }
}