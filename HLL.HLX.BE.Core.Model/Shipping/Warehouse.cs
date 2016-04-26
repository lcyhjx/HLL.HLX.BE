using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Shipping
{
    /// <summary>
    /// Represents a shipment
    /// </summary>
    public partial class Warehouse : FullAuditedEntity<int, User>
    {
        /// <summary>
        /// Gets or sets the warehouse name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the address identifier of the warehouse
        /// </summary>
        public int AddressId { get; set; }
    }
}