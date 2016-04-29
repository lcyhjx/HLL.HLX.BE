using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Discounts;

namespace HLL.HLX.BE.EntityFramework.Repositories.Discounts
{
    public class DiscountRepository : HlxBeRepositoryBase<Discount, int>, IDiscountRepository
    {
        public DiscountRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
