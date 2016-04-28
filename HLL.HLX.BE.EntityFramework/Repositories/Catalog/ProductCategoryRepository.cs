using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductCategoryRepository : HlxBeRepositoryBase<ProductCategory, int>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
