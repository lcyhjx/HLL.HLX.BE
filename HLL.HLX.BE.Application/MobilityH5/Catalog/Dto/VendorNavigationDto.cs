using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Model.Vendors;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class VendorNavigationDto 
    {
        public VendorNavigationDto()
        {
            this.Vendors = new List<VendorBriefInfoDto>();
        }

        public IList<VendorBriefInfoDto> Vendors { get; set; }

        public int TotalVendors { get; set; }
    }


    [AutoMapFrom(typeof(Vendor))]
    public partial class VendorBriefInfoDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string SeName { get; set; }
    }
}