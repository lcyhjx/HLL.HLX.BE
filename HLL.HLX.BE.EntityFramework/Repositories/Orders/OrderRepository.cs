using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class OrderRepository : HlxBeRepositoryBase<Order, int>, IOrderRepository
    {
        public OrderRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
