using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class OrderItemRepository : HlxBeRepositoryBase<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
