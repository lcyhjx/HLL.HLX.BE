using System.Collections.Generic;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class CategoryNavigationDto 
    {
        public CategoryNavigationDto()
        {
            Categories = new List<CategorySimpleDto>();
        }

        public int CurrentCategoryId { get; set; }
        public List<CategorySimpleDto> Categories { get; set; }
    }
}