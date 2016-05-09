using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class ReturnRequestActionRepository : HlxBeRepositoryBase<ReturnRequestAction, int>, IReturnRequestActionRepository
    {
        public ReturnRequestActionRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
