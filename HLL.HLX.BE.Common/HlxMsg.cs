using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Common
{
    public static class InfoMsg
    {

        public const string Media_Product_ImageAlternateTextFormat = "Picture of {0}";
        public const string Media_Product_ImageAlternateTextFormat_Details = "Picture of {0}";
        public const string Media_Product_ImageLinkTitleFormat = "Show details for {0}";
        public const string Media_Product_ImageLinkTitleFormat_Details = "Picture of {0}";

        public const string Products_Availability = "Availability";
        public const string Products_Availability_Backordering = "Out of Stock - on backorder and will be dispatched once in stock.";
        public const string Products_Availability_InStock = "In stock";
        public const string Products_Availability_InStockWithQuantity = "{0} in stock";
        public const string Products_Availability_OutOfStock = "Out of stock";


        public const string ShoppingCart_ConflictingShipmentSchedules = "Your cart has auto-ship (recurring) items with conflicting shipment schedules. Only one auto-ship schedule is allowed per order.";
        public const string ShoppingCart_Discount_CannotBeUsedWithGiftCards = "Sorry, this discount cannot be used with gift cards in the cart";
        public const string ShoppingCart_Discount_CannotBeUsedAnymore = "Sorry, you've used this discount already";
        public const string ShoppingCart_Discount_Expired = "Sorry, this offer is expired";
        public const string ShoppingCart_Discount_NotStartedYet = "Sorry, this offer is not started yet";
        
        
        



    }
}
