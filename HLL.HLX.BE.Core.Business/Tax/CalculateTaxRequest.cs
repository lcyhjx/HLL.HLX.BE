using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Common;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.Tax
{
    /// <summary>
    /// Represents a request for tax calculation
    /// </summary>
    public partial class CalculateTaxRequest
    {
        /// <summary>
        /// Gets or sets a customer
        /// </summary>
        public User Customer { get; set; }

        /// <summary>
        /// Gets or sets an address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets a tax category identifier
        /// </summary>
        public int TaxCategoryId { get; set; }
    }
}
