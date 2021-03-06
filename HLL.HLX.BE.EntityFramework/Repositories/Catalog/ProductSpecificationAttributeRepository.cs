﻿using HLL.HLX.BE.Core.Model.Catalog;
using Abp.EntityFramework;

namespace HLL.HLX.BE.EntityFramework.Repositories.Catalog
{
    public class ProductSpecificationAttributeRepository : HlxBeRepositoryBase<ProductSpecificationAttribute, int>, IProductSpecificationAttributeRepository
    {
        public ProductSpecificationAttributeRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
