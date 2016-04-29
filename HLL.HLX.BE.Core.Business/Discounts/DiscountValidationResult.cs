using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Core.Business.Discounts
{
    /// <summary>
    /// Represents a result of discount validation
    /// </summary>
    public partial class DiscountValidationResult
    {
        /// <summary>
        /// Gets or sets a alue indicating whether discount is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets an error that a customer should see when enterting a coupon code (in case if "IsValid" is set to "false")
        /// </summary>
        public string UserError { get; set; }
    }
}
