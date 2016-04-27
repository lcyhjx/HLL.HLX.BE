using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Repositories.Directory
{
    public  class MeasureWeightRepository : HlxBeRepositoryBase<MeasureWeight, int>, IMeasureWeightRepository
    {
        public MeasureWeightRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
