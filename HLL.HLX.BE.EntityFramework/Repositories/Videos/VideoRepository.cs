using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.Repositories.Videos
{
    public class VideoRepository : HlxBeRepositoryBase<Video, int>, IVideoRepository
    {
        public VideoRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
