using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.Repositories.Users
{
    public class UserAvatarRepository : HlxBeRepositoryBase<UserAvatar, int>, IUserAvatarRepository
    {
        public UserAvatarRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
