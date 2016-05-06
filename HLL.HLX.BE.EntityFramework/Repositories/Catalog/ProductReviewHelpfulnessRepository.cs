using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductReviewHelpfulnessRepository : HlxBeRepositoryBase<ProductReviewHelpfulness, int>, IProductReviewHelpfulnessRepository
    {
        public ProductReviewHelpfulnessRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
