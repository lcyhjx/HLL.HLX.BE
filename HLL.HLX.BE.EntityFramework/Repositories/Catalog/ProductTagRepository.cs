using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductTagRepository : HlxBeRepositoryBase<ProductTag, int>, IProductTagRepository
    {
        public ProductTagRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
