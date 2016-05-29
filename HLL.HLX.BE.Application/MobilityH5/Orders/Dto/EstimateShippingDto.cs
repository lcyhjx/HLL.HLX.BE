using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class EstimateShippingDto : EntityDto
    {
        public EstimateShippingDto()
        {
            ShippingOptions = new List<ShippingOptionDto>();
            Warnings = new List<string>();
            
            AvailableCountries = new List<SelectListItemDto>();
            AvailableStates = new List<SelectListItemDto>();
        }

        public bool Enabled { get; set; }

        public IList<ShippingOptionDto> ShippingOptions { get; set; }

        public IList<string> Warnings { get; set; }
        
        
        public int? CountryId { get; set; }
        
        public int? StateProvinceId { get; set; }
        
        public string ZipPostalCode { get; set; }

        public IList<SelectListItemDto> AvailableCountries { get; set; }
        public IList<SelectListItemDto> AvailableStates { get; set; }

		#region Nested Classes

        public partial class ShippingOptionDto : EntityDto
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public string Price { get; set; }
        }

		#endregion
    }
}