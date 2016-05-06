using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class SpecificationAttributeRepository : HlxBeRepositoryBase<SpecificationAttribute, int>, ISpecificationAttributeRepository
    {
        public SpecificationAttributeRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
