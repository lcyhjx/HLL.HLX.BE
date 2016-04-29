using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.EntityFramework.Repositories.Common
{
    public class GenericAttributeRepository : HlxBeRepositoryBase<GenericAttribute, int>, IGenericAttributeRepository
    {
        public GenericAttributeRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
