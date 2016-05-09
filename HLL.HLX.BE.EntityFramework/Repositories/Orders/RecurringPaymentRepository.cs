using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class RecurringPaymentRepository : HlxBeRepositoryBase<RecurringPayment, int>, IRecurringPaymentRepository
    {
        public RecurringPaymentRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
