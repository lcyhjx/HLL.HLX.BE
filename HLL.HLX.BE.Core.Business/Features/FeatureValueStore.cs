using Abp.Application.Features;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model.Authorization.Roles;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}