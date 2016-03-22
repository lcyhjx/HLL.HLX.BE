using Abp.Web.Mvc.Views;
using HLL.HLX.BE.Core;
using HLL.HLX.BE.Core.Business;
using HLL.HLX.BE.Core.Model;

namespace HLL.HLX.BE.Web.Views
{
    public abstract class HlxBeWebViewPageBase : HlxBeWebViewPageBase<dynamic>
    {

    }

    public abstract class HlxBeWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected HlxBeWebViewPageBase()
        {
            LocalizationSourceName = HlxBeConsts.LocalizationSourceName;
        }
    }
}