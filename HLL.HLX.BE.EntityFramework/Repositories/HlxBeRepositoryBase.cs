using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace HLL.HLX.BE.EntityFramework.Repositories
{
    public abstract class HlxBeRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<HlxBeDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected HlxBeRepositoryBase(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class HlxBeRepositoryBase<TEntity> : HlxBeRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected HlxBeRepositoryBase(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
