using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.Core.Business.Orders
{
    /// <summary>
    /// Applied gift card
    /// </summary>
    public class AppliedGiftCard
    {
        /// <summary>
        /// Gets or sets the used value
        /// </summary>
        public decimal AmountCanBeUsed { get; set; }

        /// <summary>
        /// Gets the gift card
        /// </summary>
        public GiftCard GiftCard { get; set; }
    }
}
