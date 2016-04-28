using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class BackInStockSubscriptionRepository : HlxBeRepositoryBase<BackInStockSubscription, int>, IBackInStockSubscriptionRepository
    {
        public BackInStockSubscriptionRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
