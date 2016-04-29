using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Discounts;

namespace HLL.HLX.BE.EntityFramework.Repositories.Discounts
{
    public class DiscountRequirementRepository : HlxBeRepositoryBase<DiscountRequirement, int>, IDiscountRequirementRepository
    {
        public DiscountRequirementRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
