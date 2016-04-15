using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput : IOutputDto
    {
        public UserLoginInfoDto User { get; set; }
        public TenantLoginInfoDto Tenant { get; set; }
    }
}