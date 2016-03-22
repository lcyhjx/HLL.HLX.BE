using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Model.MultiTenancy;

namespace HLL.HLX.BE.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantListDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}