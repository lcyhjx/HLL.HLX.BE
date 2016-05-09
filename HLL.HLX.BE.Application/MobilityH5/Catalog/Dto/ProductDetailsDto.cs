using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{

    [AutoMapFrom(typeof(Product))]    
    public partial class ProductDetailsDto : EntityDto<int>
    {
        public ProductDetailsDto()
        {
            DefaultPictureModel = new PictureDto();
            PictureModels = new List<PictureDto>();
            GiftCard = new GiftCardDto();
            ProductPrice = new ProductPriceDto();
            AddToCart = new AddToCartDto();
            ProductAttributes = new List<ProductAttributeDto>();
            AssociatedProducts = new List<ProductDetailsDto>();
            VendorDto = new VendorBriefInfoDto();
            Breadcrumb = new ProductBreadcrumbDto();
            ProductTags = new List<ProductTagDto>();
            ProductSpecifications = new List<ProductSpecificationDto>();
            ProductManufacturers = new List<ManufacturerDto>();
            ProductReviewOverview = new ProductReviewOverviewDto();
            TierPrices = new List<TierPriceDto>();
        }

        //picture(s)
        public bool DefaultPictureZoomEnabled { get; set; }
        public PictureDto DefaultPictureModel { get; set; }
        public IList<PictureDto> PictureModels { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ProductTemplateViewPath { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }

        public bool ShowSku { get; set; }
        public string Sku { get; set; }

        public bool ShowManufacturerPartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }

        public bool ShowGtin { get; set; }
        public string Gtin { get; set; }

        public bool ShowVendor { get; set; }
        public VendorBriefInfoDto VendorDto { get; set; }

        public bool HasSampleDownload { get; set; }

        public GiftCardDto GiftCard { get; set; }

        public bool IsShipEnabled { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool FreeShippingNotificationEnabled { get; set; }
        public string DeliveryDate { get; set; }


        public bool IsRental { get; set; }
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }

        public string StockAvailability { get; set; }

        public bool DisplayBackInStockSubscription { get; set; }

        public bool EmailAFriendEnabled { get; set; }
        public bool CompareProductsEnabled { get; set; }

        public string PageShareCode { get; set; }

        public ProductPriceDto ProductPrice { get; set; }

        public AddToCartDto AddToCart { get; set; }

        public ProductBreadcrumbDto Breadcrumb { get; set; }

        public IList<ProductTagDto> ProductTags { get; set; }

        public IList<ProductAttributeDto> ProductAttributes { get; set; }

        public IList<ProductSpecificationDto> ProductSpecifications { get; set; }

        public IList<ManufacturerDto> ProductManufacturers { get; set; }

        public ProductReviewOverviewDto ProductReviewOverview { get; set; }

        public IList<TierPriceDto> TierPrices { get; set; }

        //a list of associated products. For example, "Grouped" products could have several child "simple" products
        public IList<ProductDetailsDto> AssociatedProducts { get; set; }

        public bool DisplayDiscontinuedMessage { get; set; }

        #region Nested Classes

        public partial class ProductBreadcrumbDto : EntityDto<int>
        {
            public ProductBreadcrumbDto()
            {
                CategoryBreadcrumb = new List<CategorySimpleDto>();
            }

            public bool Enabled { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductSeName { get; set; }
            public IList<CategorySimpleDto> CategoryBreadcrumb { get; set; }
        }

        public partial class AddToCartDto : EntityDto<int>
        {
            public AddToCartDto()
            {
                this.AllowedQuantities = new List<int>();
            }
            public int ProductId { get; set; }

            //qty
            public int EnteredQuantity { get; set; }
            public string MinimumQuantityNotification { get; set; }
            public List<int> AllowedQuantities { get; set; }

            //price entered by customers
            public bool CustomerEntersPrice { get; set; }
            public decimal CustomerEnteredPrice { get; set; }
            public String CustomerEnteredPriceRange { get; set; }

            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }

            //rental
            public bool IsRental { get; set; }

            //pre-order
            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            //updating existing shopping cart item?
            public int UpdatedShoppingCartItemId { get; set; }
        }

        public partial class ProductPriceDto : EntityDto<int>
        {
            /// <summary>
            /// The currency (in 3-letter ISO 4217 format) of the offer price 
            /// </summary>
            public string CurrencyCode { get; set; }

            public string OldPrice { get; set; }

            public string Price { get; set; }
            public string PriceWithDiscount { get; set; }
            public decimal PriceValue { get; set; }

            public bool CustomerEntersPrice { get; set; }

            public bool CallForPrice { get; set; }

            public int ProductId { get; set; }

            public bool HidePrices { get; set; }

            //rental
            public bool IsRental { get; set; }
            public string RentalPrice { get; set; }

            /// <summary>
            /// A value indicating whether we should display tax/shipping info (used in Germany)
            /// </summary>
            public bool DisplayTaxShippingInfo { get; set; }
            /// <summary>
            /// PAngV baseprice (used in Germany)
            /// </summary>
            public string BasePricePAngV { get; set; }
        }

        public partial class GiftCardDto : EntityDto<int>
        {
            public bool IsGiftCard { get; set; }
            public string RecipientName { get; set; }
            
            public string RecipientEmail { get; set; }
            
            public string SenderName { get; set; }
            
            public string SenderEmail { get; set; }
            
            public string Message { get; set; }

            public GiftCardType GiftCardType { get; set; }
        }

        public partial class TierPriceDto : EntityDto<int>
        {
            public string Price { get; set; }

            public int Quantity { get; set; }
        }

        public partial class ProductAttributeDto : EntityDto<int>
        {
            public ProductAttributeDto()
            {
                AllowedFileExtensions = new List<string>();
                Values = new List<ProductAttributeValueDto>();
            }

            public int ProductId { get; set; }

            public int ProductAttributeId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }
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
            /// A value indicating whether this attribute depends on some other attribute
            /// </summary>
            public bool HasCondition { get; set; }

            /// <summary>
            /// Allowed file extensions for customer uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<ProductAttributeValueDto> Values { get; set; }

        }

        public partial class ProductAttributeValueDto : EntityDto<int>
        {
            public ProductAttributeValueDto()
            {
                PictureModel = new PictureDto();
            }

            public string Name { get; set; }

            public string ColorSquaresRgb { get; set; }

            public string PriceAdjustment { get; set; }

            public decimal PriceAdjustmentValue { get; set; }

            public bool IsPreSelected { get; set; }

            //picture model is used when we want to override a default product picture when some attribute is selected
            public PictureDto PictureModel { get; set; }
        }

        #endregion
    }
}
