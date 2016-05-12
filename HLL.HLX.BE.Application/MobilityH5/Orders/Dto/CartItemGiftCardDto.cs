using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public class CartItemGiftCardDto
    {
        /// <summary>
        /// Gets or sets a recipient name
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or sets a recipient email
        /// </summary>
        public string RecipientEmail { get; set; }

        /// <summary>
        /// Gets or sets a sender name
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets a sender email
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets a message
        /// </summary>
        public string Message { get; set; }
    }
}
