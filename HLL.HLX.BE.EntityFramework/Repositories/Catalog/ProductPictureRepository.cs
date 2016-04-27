using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductPictureRepository : HlxBeRepositoryBase<ProductPicture, int>, IProductPictureRepository
    {
        public ProductPictureRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
