using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class RecurringPaymentHistoryRepository : HlxBeRepositoryBase<RecurringPaymentHistory, int>, IRecurringPaymentHistoryRepository
    {
        public RecurringPaymentHistoryRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
