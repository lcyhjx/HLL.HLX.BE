using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.MultiTenancy.Dto;
using HLL.HLX.BE.MultiTenancy.Dto;

namespace HLL.HLX.BE.Application.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();
        Task CreateTenant(CreateTenantInput input);
    }
}