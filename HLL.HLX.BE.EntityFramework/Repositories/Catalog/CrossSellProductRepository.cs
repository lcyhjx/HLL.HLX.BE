using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class CrossSellProductRepository : HlxBeRepositoryBase<CrossSellProduct, int>, ICrossSellProductRepository
    {
        public CrossSellProductRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
