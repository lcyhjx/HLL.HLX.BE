using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using HLL.HLX.BE.Application.Roles.Dto;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Roles;

namespace HLL.HLX.BE.Application.Roles
{
    /* THIS IS JUST A SAMPLE. */

    public class RoleAppService : HlxBeAppServiceBase, IRoleAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}