using Abp.Authorization;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Core;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model.Authorization.Roles;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
