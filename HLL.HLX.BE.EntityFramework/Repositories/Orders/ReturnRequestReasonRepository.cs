using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class ReturnRequestReasonRepository : HlxBeRepositoryBase<ReturnRequestReason, int>, IReturnRequestReasonRepository
    {
        public ReturnRequestReasonRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
