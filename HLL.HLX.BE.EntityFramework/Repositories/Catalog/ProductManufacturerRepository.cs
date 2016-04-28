using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductManufacturerRepository : HlxBeRepositoryBase<ProductManufacturer, int>, IProductManufacturerRepository
    {
        public ProductManufacturerRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
