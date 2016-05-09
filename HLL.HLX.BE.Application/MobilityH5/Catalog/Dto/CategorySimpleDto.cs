using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public class CategorySimpleDto : EntityDto<int>
    {
        public CategorySimpleDto()
        {
            SubCategories = new List<CategorySimpleDto>();
        }

        public string Name { get; set; }

        public string SeName { get; set; }

        public int? NumberOfProducts { get; set; }

        public bool IncludeInTopMenu { get; set; }

        public List<CategorySimpleDto> SubCategories { get; set; }
    }
}