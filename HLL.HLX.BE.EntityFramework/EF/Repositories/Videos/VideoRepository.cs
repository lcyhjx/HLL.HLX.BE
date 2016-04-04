using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.EF.Repositories.Videos
{
    public class VideoRepository : HlxBeRepositoryBase<Video, long>, IVideoRepository
    {
        public VideoRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
