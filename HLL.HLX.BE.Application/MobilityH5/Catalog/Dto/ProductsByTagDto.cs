using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class ProductsByTagDto : EntityDto<int>
    {
        public ProductsByTagDto()
        {
            Products = new List<ProductOverviewDto>();
            //PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string TagName { get; set; }
        public string TagSeName { get; set; }
        
        //public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewDto> Products { get; set; }
    }
}