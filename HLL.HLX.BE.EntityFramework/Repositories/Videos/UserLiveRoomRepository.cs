using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.Repositories.Videos
{
    public class UserLiveRoomRepository : HlxBeRepositoryBase<UserLiveRoom, int>, IUserLiveRoomRepository
    {
        public UserLiveRoomRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
