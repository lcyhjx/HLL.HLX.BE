using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.EF.Repositories.Users
{
    public class UserAvatarRepository : HlxBeRepositoryBase<UserAvatar, long>, IUserAvatarRepository
    {
        public UserAvatarRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
