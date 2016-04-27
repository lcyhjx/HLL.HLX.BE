using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Repositories.Directory
{
    public class MeasureDimensionRepository : HlxBeRepositoryBase<MeasureDimension, int>, IMeasureDimensionRepository
    {
        public MeasureDimensionRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
