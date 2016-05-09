using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class ShoppingCartItemRepository : HlxBeRepositoryBase<ShoppingCartItem, int>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
