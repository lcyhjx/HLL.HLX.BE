using Abp.Authorization.Roles;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Authorization.Roles
{
    public class Role : AbpRole<Tenant, User>
    {

    }
}