using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class CompareProductsDto : EntityDto<int>
    {
        public CompareProductsDto()
        {
            Products = new List<ProductOverviewDto>();
        }
        public IList<ProductOverviewDto> Products { get; set; }

        public bool IncludeShortDescriptionInCompareProducts { get; set; }
        public bool IncludeFullDescriptionInCompareProducts { get; set; }
    }
}