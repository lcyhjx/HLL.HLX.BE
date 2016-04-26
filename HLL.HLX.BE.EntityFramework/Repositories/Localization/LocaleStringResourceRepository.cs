using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Localization;

namespace HLL.HLX.BE.EntityFramework.Repositories.Localization
{
    public class LocaleStringResourceRepository : HlxBeRepositoryBase<LocaleStringResource, int>, ILocaleStringResourceRepository
    {
        public LocaleStringResourceRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
