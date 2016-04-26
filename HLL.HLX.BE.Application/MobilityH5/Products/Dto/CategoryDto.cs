using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class CategoryDto : EntityDto<int>
    {
        public CategoryDto()
        {
            PictureModel = new PictureDto();
            FeaturedProducts = new List<ProductOverviewDto>();
            Products = new List<ProductOverviewDto>();
            //PagingFilteringContext = new CatalogPagingFilteringModel();
            SubCategories = new List<SubCategoryDto>();
            CategoryBreadcrumb = new List<CategoryDto>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        
        public PictureDto PictureModel { get; set; }

        //public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public bool DisplayCategoryBreadcrumb { get; set; }
        public IList<CategoryDto> CategoryBreadcrumb { get; set; }
        
        public IList<SubCategoryDto> SubCategories { get; set; }

        public IList<ProductOverviewDto> FeaturedProducts { get; set; }
        public IList<ProductOverviewDto> Products { get; set; }
        

		#region Nested Classes

        public partial class SubCategoryDto : EntityDto<int>
        {
            public SubCategoryDto()
            {
                PictureModel = new PictureDto();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public string Description { get; set; }

            public PictureDto PictureModel { get; set; }
        }

		#endregion
    }
}