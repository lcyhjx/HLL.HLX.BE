using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class GiftCardUsageHistoryRepository : HlxBeRepositoryBase<GiftCardUsageHistory, int>, IGiftCardUsageHistoryRepository
    {
        public GiftCardUsageHistoryRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
