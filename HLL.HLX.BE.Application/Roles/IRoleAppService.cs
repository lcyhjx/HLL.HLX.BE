using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Roles.Dto;

namespace HLL.HLX.BE.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
