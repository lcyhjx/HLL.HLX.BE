using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.EntityFramework.Repositories.Shipping
{
    public class DeliveryDateRepository : HlxBeRepositoryBase<DeliveryDate, int>, IDeliveryDateRepository
    {
        public DeliveryDateRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
