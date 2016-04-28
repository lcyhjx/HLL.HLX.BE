using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ManufacturerRepository : HlxBeRepositoryBase<Manufacturer, int>, IManufacturerRepository
    {
        public ManufacturerRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
