using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Seo;

namespace HLL.HLX.BE.EntityFramework.Repositories.Seo
{
    public class UrlRecordRepository : HlxBeRepositoryBase<UrlRecord, int>, IUrlRecordRepository
    {
        public UrlRecordRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
