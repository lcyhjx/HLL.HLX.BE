using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductWarehouseInventoryRepository : HlxBeRepositoryBase<ProductWarehouseInventory, int>, IProductWarehouseInventoryRepository
    {
        public ProductWarehouseInventoryRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
