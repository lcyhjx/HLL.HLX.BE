using System.Collections.Generic;
using System.Linq;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.EF.Repositories.Users
{
    public class UserRepository : HlxBeRepositoryBase<User, long>, IUserRepository
    {
        public UserRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        
    }
}
