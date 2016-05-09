using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class CheckoutAttributeValueRepository : HlxBeRepositoryBase<CheckoutAttributeValue, int>, ICheckoutAttributeValueRepository
    {
        public CheckoutAttributeValueRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
