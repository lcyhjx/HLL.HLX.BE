using System.Threading.Tasks;
using Abp.Authorization;
using HLL.HLX.BE.Application.Users.Dto;
using HLL.HLX.BE.Core.Business.Users;

namespace HLL.HLX.BE.Application.Users
{
    /* THIS IS JUST A SAMPLE. */

    public class UserAppService : HlxBeAppServiceBase, IUserAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly UserManager _userManager;

        public UserAppService(UserManager userManager, IPermissionManager permissionManager)
        {
            _userManager = userManager;
            _permissionManager = permissionManager;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await _userManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(userId, roleName));
        }
    }
}