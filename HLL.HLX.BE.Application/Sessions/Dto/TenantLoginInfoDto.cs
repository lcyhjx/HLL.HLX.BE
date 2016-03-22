using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.MultiTenancy;

namespace HLL.HLX.BE.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}