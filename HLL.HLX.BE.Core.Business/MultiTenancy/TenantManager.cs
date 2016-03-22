using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model.Authorization.Roles;
using HLL.HLX.BE.Core.Model.Editions;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager
            )
        {
        }
    }
}