using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Configuration;

namespace HLL.HLX.BE.EntityFramework.Repositories.Configuration
{
    public class ParaSettingRepository : HlxBeRepositoryBase<ParaSetting, int>, IParaSettingRepository
    {
        public ParaSettingRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
