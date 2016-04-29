using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Discounts;

namespace HLL.HLX.BE.EntityFramework.Repositories.Discounts
{
    public class DiscountUsageHistoryRepository : HlxBeRepositoryBase<DiscountUsageHistory, int>, IDiscountUsageHistoryRepository
    {
        public DiscountUsageHistoryRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
