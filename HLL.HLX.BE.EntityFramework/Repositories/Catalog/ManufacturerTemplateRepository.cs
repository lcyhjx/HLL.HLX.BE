using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ManufacturerTemplateRepository : HlxBeRepositoryBase<ManufacturerTemplate, int>, IManufacturerTemplateRepository
    {
        public ManufacturerTemplateRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
