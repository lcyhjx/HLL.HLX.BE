﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class EstimateShippingModel : EntityDto
    {
        public EstimateShippingModel()
        {
            ShippingOptions = new List<ShippingOptionModel>();
            Warnings = new List<string>();
            
            //AvailableCountries = new List<SelectListItem>();
            //AvailableStates = new List<SelectListItem>();
        }

        public bool Enabled { get; set; }

        public IList<ShippingOptionModel> ShippingOptions { get; set; }

        public IList<string> Warnings { get; set; }
        
        
        public int? CountryId { get; set; }
        
        public int? StateProvinceId { get; set; }
        
        public string ZipPostalCode { get; set; }

        //public IList<SelectListItem> AvailableCountries { get; set; }
        //public IList<SelectListItem> AvailableStates { get; set; }

		#region Nested Classes

        public partial class ShippingOptionModel : EntityDto
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public string Price { get; set; }
        }

		#endregion
    }
}