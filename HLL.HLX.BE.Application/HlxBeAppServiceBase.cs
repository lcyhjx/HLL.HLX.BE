using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using HLL.HLX.BE.Core.Business.MultiTenancy;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Application
{
    /// <summary>
    ///     Derive your application services from this class.
    /// </summary>
    public abstract class HlxBeAppServiceBase : ApplicationService
    {
        protected HlxBeAppServiceBase()
        {
            LocalizationSourceName = HlxBeConsts.LocalizationSourceName;
        }

        public TenantManager TenantManager { get; set; }
        public UserManager UserManager { get; set; }

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

        private User _cachedUser;
        protected virtual User CurrentUser
        {
            get
            {
                if (_cachedUser == null || _cachedUser.Id != AbpSession.UserId)
                {
                    var user = UserManager.FindById(AbpSession.GetUserId());
                    if (user == null)
                    {
                        throw new ApplicationException("There is no current user!");
                    }
                    _cachedUser = user;
                    return user;
                }
                return _cachedUser;
            }
        }


        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}