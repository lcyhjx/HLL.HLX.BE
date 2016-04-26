using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Vendors;

namespace HLL.HLX.BE.EntityFramework.Repositories.Vendors
{
    public class VendorRepository : HlxBeRepositoryBase<Vendor, int>, IVendorRepository
    {
        public VendorRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
