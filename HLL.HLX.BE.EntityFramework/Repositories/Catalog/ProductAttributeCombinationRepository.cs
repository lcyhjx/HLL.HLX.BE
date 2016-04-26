using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductAttributeCombinationRepository : HlxBeRepositoryBase<ProductAttributeCombination, int>, IProductAttributeCombinationRepository
    {
        public ProductAttributeCombinationRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
