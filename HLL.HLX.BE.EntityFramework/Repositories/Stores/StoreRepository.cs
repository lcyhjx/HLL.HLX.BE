using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.EntityFramework.Repositories.Stores
{
    public class StoreRepository : HlxBeRepositoryBase<Store, int>, IStoreRepository
    {
        public StoreRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
