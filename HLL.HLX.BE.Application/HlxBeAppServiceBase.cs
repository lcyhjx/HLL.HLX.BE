using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using HLL.HLX.BE.Core;
using HLL.HLX.BE.Core.Business;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;
using HLL.HLX.BE.MultiTenancy;
using HLL.HLX.BE.Users;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class HlxBeAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected HlxBeAppServiceBase()
        {
            LocalizationSourceName = HlxBeConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}