using System;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Vendors
{
    /// <summary>
    /// Represents a vendor note
    /// </summary>
    public partial class VendorNote : FullAuditedEntity<int, User>
    {
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the date and time of vendor note creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets the vednor
        /// </summary>
        public virtual Vendor Vendor { get; set; }
    }

}
