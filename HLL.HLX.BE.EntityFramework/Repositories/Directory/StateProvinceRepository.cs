using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Repositories.Directory
{
    public class StateProvinceRepository : HlxBeRepositoryBase<StateProvince, int>, IStateProvinceRepository
    {
        public StateProvinceRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
