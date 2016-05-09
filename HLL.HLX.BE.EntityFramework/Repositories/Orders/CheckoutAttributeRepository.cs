using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Repositories.Orders
{
    public class CheckoutAttributeRepository : HlxBeRepositoryBase<CheckoutAttribute, int>, ICheckoutAttributeRepository
    {
        public CheckoutAttributeRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
