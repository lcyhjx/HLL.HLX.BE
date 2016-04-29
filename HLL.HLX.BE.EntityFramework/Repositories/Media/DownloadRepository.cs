using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Media;

namespace HLL.HLX.BE.EntityFramework.Repositories.Media
{
    public  class DownloadRepository : HlxBeRepositoryBase<Download, int>, IDownloadRepository
    {
        public DownloadRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
