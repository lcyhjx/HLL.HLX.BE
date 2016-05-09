using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    [AutoMapFrom(typeof(Manufacturer))]
    public partial class ManufacturerDto : EntityDto<int>
    {
        public ManufacturerDto()
        {
            PictureModel = new PictureDto();
            FeaturedProducts = new List<ProductOverviewDto>();
            Products = new List<ProductOverviewDto>();
            //PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }


        public PictureDto PictureModel { get; set; }

        //public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewDto> FeaturedProducts { get; set; }
        public IList<ProductOverviewDto> Products { get; set; }
    }
}