using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class SpecificationAttributeOptionRepository : HlxBeRepositoryBase<SpecificationAttributeOption, int>, ISpecificationAttributeOptionRepository
    {
        public SpecificationAttributeOptionRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
