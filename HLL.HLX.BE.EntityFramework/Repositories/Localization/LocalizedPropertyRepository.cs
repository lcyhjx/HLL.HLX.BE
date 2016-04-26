using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Localization;

namespace HLL.HLX.BE.EntityFramework.Repositories.Localization
{
    public class LocalizedPropertyRepository : HlxBeRepositoryBase<LocalizedProperty, int>, ILocalizedPropertyRepository
    {
        public LocalizedPropertyRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
