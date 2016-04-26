using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.Repositories.Users
{
    public class UserRepository : HlxBeRepositoryBase<User, long>, IUserRepository
    {
        public UserRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        
    }
}
