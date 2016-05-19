using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;
using HLL.HLX.BE.Application.MobilityH5.Common.Dto;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class ShoppingCartModel : EntityDto
    {
        public ShoppingCartModel()
        {
            Items = new List<ShoppingCartItemModel>();
            Warnings = new List<string>();
            EstimateShipping = new EstimateShippingModel();
            DiscountBox = new DiscountBoxModel();
            GiftCardBox = new GiftCardBoxModel();
            CheckoutAttributes = new List<CheckoutAttributeModel>();
            OrderReviewData = new OrderReviewDataModel();

            ButtonPaymentMethodActionNames = new List<string>();
            ButtonPaymentMethodControllerNames = new List<string>();
            //ButtonPaymentMethodRouteValues = new List<RouteValueDictionary>();
        }

        public bool OnePageCheckoutEnabled { get; set; }

        public bool ShowSku { get; set; }
        public bool ShowProductImages { get; set; }
        public bool IsEditable { get; set; }
        public IList<ShoppingCartItemModel> Items { get; set; }

        public string CheckoutAttributeInfo { get; set; }
        public IList<CheckoutAttributeModel> CheckoutAttributes { get; set; }

        public IList<string> Warnings { get; set; }
        public string MinOrderSubtotalWarning { get; set; }
        public bool DisplayTaxShippingInfo { get; set; }
        public bool TermsOfServiceOnShoppingCartPage { get; set; }
        public bool TermsOfServiceOnOrderConfirmPage { get; set; }
        public EstimateShippingModel EstimateShipping { get; set; }
        public DiscountBoxModel DiscountBox { get; set; }
        public GiftCardBoxModel GiftCardBox { get; set; }
        public OrderReviewDataModel OrderReviewData { get; set; }

        public IList<string> ButtonPaymentMethodActionNames { get; set; }
        public IList<string> ButtonPaymentMethodControllerNames { get; set; }
        //public IList<RouteValueDictionary> ButtonPaymentMethodRouteValues { get; set; }

		#region Nested Classes

        public partial class ShoppingCartItemModel : EntityDto
        {
            public ShoppingCartItemModel()
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

        public partial class CheckoutAttributeModel : EntityDto
        {
            public CheckoutAttributeModel()
            {
                AllowedFileExtensions = new List<string>();
                Values = new List<CheckoutAttributeValueModel>();
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

            public IList<CheckoutAttributeValueModel> Values { get; set; }
        }

        public partial class CheckoutAttributeValueModel : EntityDto
        {
            public string Name { get; set; }

            public string ColorSquaresRgb { get; set; }

            public string PriceAdjustment { get; set; }

            public bool IsPreSelected { get; set; }
        }

        public partial class DiscountBoxModel
        {
            public bool Display { get; set; }
            public string Message { get; set; }
            public string CurrentCode { get; set; }
            public bool IsApplied { get; set; }
        }

        public partial class GiftCardBoxModel 
        {
            public bool Display { get; set; }
            public string Message { get; set; }
            public bool IsApplied { get; set; }
        }

        public partial class OrderReviewDataModel 
        {
            public OrderReviewDataModel()
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