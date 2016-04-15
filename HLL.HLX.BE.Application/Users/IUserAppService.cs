using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.Users.Dto;

namespace HLL.HLX.BE.Application.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);
        Task RemoveFromRole(long userId, string roleName);
    }
}