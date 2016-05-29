using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;
using HLL.HLX.BE.Application.MobilityH5.Common.Dto;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class ShoppingCartDto : EntityDto
    {
        public ShoppingCartDto()
        {
            Items = new List<ShoppingCartItemDto>();
            Warnings = new List<string>();
            EstimateShipping = new EstimateShippingDto();
            DiscountBox = new DiscountBoxDto();
            GiftCardBox = new GiftCardBoxDto();
            CheckoutAttributes = new List<CheckoutAttributeDto>();
            OrderReviewData = new OrderReviewDataDto();

            ButtonPaymentMethodActionNames = new List<string>();
            ButtonPaymentMethodControllerNames = new List<string>();
            //ButtonPaymentMethodRouteValues = new List<RouteValueDictionary>();
        }

        public bool OnePageCheckoutEnabled { get; set; }

        public bool ShowSku { get; set; }
        public bool ShowProductImages { get; set; }
        public bool IsEditable { get; set; }
        public IList<ShoppingCartItemDto> Items { get; set; }

        public string CheckoutAttributeInfo { get; set; }
        public IList<CheckoutAttributeDto> CheckoutAttributes { get; set; }

        public IList<string> Warnings { get; set; }
        public string MinOrderSubtotalWarning { get; set; }
        public bool DisplayTaxShippingInfo { get; set; }
        public bool TermsOfServiceOnShoppingCartPage { get; set; }
        public bool TermsOfServiceOnOrderConfirmPage { get; set; }
        public EstimateShippingDto EstimateShipping { get; set; }
        public DiscountBoxDto DiscountBox { get; set; }
        public GiftCardBoxDto GiftCardBox { get; set; }
        public OrderReviewDataDto OrderReviewData { get; set; }

        public IList<string> ButtonPaymentMethodActionNames { get; set; }
        public IList<string> ButtonPaymentMethodControllerNames { get; set; }
        //public IList<RouteValueDictionary> ButtonPaymentMethodRouteValues { get; set; }

		#region Nested Classes

        public partial class ShoppingCartItemDto : EntityDto
        {
            public ShoppingCartItemDto()
            {
                Picture = new PictureDto();
                AllowedQuantities = new List<int>();
                Warnings = new List<string>();
            }
            public string Sku { get; set; }

            public PictureDto Picture {get;set;}

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductSeName { get; set; }

            public string UnitPrice { get; set; }

            public string SubTotal { get; set; }

            public string Discount { get; set; }

            public int Quantity { get; set; }

            public List<int> AllowedQuantities { get; set; }
            
            public string AttributeInfo { get; set; }

            public string RecurringInfo { get; set; }

            public string RentalInfo { get; set; }

            public bool AllowItemEditing { get; set; }

            public IList<string> Warnings { get; set; }

        }

        public partial class CheckoutAttributeDto : EntityDto
        {
            public CheckoutAttributeDto()
            {
                AllowedFileExtensions = new List<string>();
                Values = new List<CheckoutAttributeValueDto>();
            }

            public string Name { get; set; }

            public string DefaultValue { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Selected day value for datepicker
            /// </summary>
            public int? SelectedDay { get; set; }
            /// <summary>
            /// Selected month value for datepicker
            /// </summary>
            public int? SelectedMonth { get; set; }
            /// <summary>
            /// Selected year value for datepicker
            /// </summary>
            public int? SelectedYear { get; set; }

            /// <summary>
            /// Allowed file extensions for customer uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<CheckoutAttributeValueDto> Values { get; set; }
        }

        public partial class CheckoutAttributeValueDto : EntityDto
        {
            public string Name { get; set; }

            public string ColorSquaresRgb { get; set; }

            public string PriceAdjustment { get; set; }

            public bool IsPreSelected { get; set; }
        }

        public partial class DiscountBoxDto
        {
            public bool Display { get; set; }
            public string Message { get; set; }
            public string CurrentCode { get; set; }
            public bool IsApplied { get; set; }
        }

        public partial class GiftCardBoxDto 
        {
            public bool Display { get; set; }
            public string Message { get; set; }
            public bool IsApplied { get; set; }
        }

        public partial class OrderReviewDataDto 
        {
            public OrderReviewDataDto()
            {
                this.BillingAddress = new AddressDto();
                this.ShippingAddress = new AddressDto();
                this.CustomValues= new Dictionary<string, object>();
            }
            public bool Display { get; set; }

            public AddressDto BillingAddress { get; set; }

            public bool IsShippable { get; set; }
            public AddressDto ShippingAddress { get; set; }
            public bool SelectedPickUpInStore { get; set; }
            public string ShippingMethod { get; set; }

            public string PaymentMethod { get; set; }

            public Dictionary<string, object> CustomValues { get; set; }
        }
		#endregion
    }
}