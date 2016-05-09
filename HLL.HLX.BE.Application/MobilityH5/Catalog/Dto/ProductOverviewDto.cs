using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class ProductOverviewDto : EntityDto<int>
    {
        public ProductOverviewDto()
        {
            ProductPrice = new ProductPriceModel();
            DefaultPictureModel = new PictureDto();
            SpecificationAttributeModels = new List<ProductSpecificationDto>();
            ReviewOverviewModel = new ProductReviewOverviewDto();
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string SeName { get; set; }

        public bool MarkAsNew { get; set; }

        //price
        public ProductPriceModel ProductPrice { get; set; }
        //picture
        public PictureDto DefaultPictureModel { get; set; }
        //specification attributes
        public IList<ProductSpecificationDto> SpecificationAttributeModels { get; set; }
        //price
        public ProductReviewOverviewDto ReviewOverviewModel { get; set; }

		#region Nested Classes

        public partial class ProductPriceModel 
        {
            public string OldPrice { get; set; }
            public string Price { get; set; }
            public decimal PriceValue { get; set; }

            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }
            public bool DisableAddToCompareListButton { get; set; }

            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            public bool IsRental { get; set; }

            public bool ForceRedirectionAfterAddingToCart { get; set; }

            /// <summary>
            /// A value indicating whether we should display tax/shipping info (used in Germany)
            /// </summary>
            public bool DisplayTaxShippingInfo { get; set; }
        }

		#endregion
    }
}