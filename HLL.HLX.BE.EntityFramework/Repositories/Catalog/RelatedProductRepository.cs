using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class RelatedProductRepository : HlxBeRepositoryBase<RelatedProduct, int>, IRelatedProductRepository
    {
        public RelatedProductRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
