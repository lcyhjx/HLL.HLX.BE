using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class SearchDto 
    {
        public SearchDto()
        {
           // this.PagingFilteringContext = new CatalogPagingFilteringModel();
            this.Products = new List<ProductOverviewDto>();

            //this.AvailableCategories = new List<SelectListItem>();
           // this.AvailableManufacturers = new List<SelectListItem>();
        }

        public string Warning { get; set; }

        public bool NoResults { get; set; }

        /// <summary>
        /// Query string
        /// </summary>
        public string q { get; set; }
        /// <summary>
        /// Category ID
        /// </summary>
        public int cid { get; set; }
        public bool isc { get; set; }
        /// <summary>
        /// Manufacturer ID
        /// </summary>
        public int mid { get; set; }
        /// <summary>
        /// Price - From 
        /// </summary>
   
        public string pf { get; set; }
        /// <summary>
        /// Price - To
        /// </summary>
  
        public string pt { get; set; }
        /// <summary>
        /// A value indicating whether to search in descriptions
        /// </summary>
     
        public bool sid { get; set; }
        /// <summary>
        /// A value indicating whether "advanced search" is enabled
        /// </summary>
        //[NopResourceDisplayName("Search.AdvancedSearch")]
        public bool adv { get; set; }
        //public IList<SelectListItem> AvailableCategories { get; set; }
        //public IList<SelectListItem> AvailableManufacturers { get; set; }


        //public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<ProductOverviewDto> Products { get; set; }

        #region Nested classes

        public class CategoryModel : EntityDto<int>
        {
            public string Breadcrumb { get; set; }
        }

        #endregion
    }
}