using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class CategoryTemplateRepository : HlxBeRepositoryBase<CategoryTemplate, int>, ICategoryTemplateRepository
    {
        public CategoryTemplateRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
