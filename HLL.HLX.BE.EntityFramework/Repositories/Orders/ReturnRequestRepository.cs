using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class ReturnRequestRepository : HlxBeRepositoryBase<ReturnRequest, int>, IReturnRequestRepository
    {
        public ReturnRequestRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
