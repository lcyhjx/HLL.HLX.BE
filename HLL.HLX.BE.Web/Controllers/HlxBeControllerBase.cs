using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using HLL.HLX.BE.Core;
using HLL.HLX.BE.Core.Business;
using HLL.HLX.BE.Core.Model;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class HlxBeControllerBase : AbpController
    {
        protected HlxBeControllerBase()
        {
            LocalizationSourceName = HlxBeConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}