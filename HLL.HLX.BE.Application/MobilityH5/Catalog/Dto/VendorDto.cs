using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class VendorDto : EntityDto<int>
    {
        public VendorDto()
        {
            PictureModel = new PictureDto();
            Products = new List<ProductOverviewDto>();
            //PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public bool AllowCustomersToContactVendors { get; set; }

        public PictureDto PictureModel { get; set; }

        //public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewDto> Products { get; set; }
    }
}