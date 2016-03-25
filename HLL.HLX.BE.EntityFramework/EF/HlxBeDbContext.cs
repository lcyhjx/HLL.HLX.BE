using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using HLL.HLX.BE.Core.Model.Authorization.Roles;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;
using HLL.HLX.BE.EntityFramework.EF.DbConfiguration;

namespace HLL.HLX.BE.EntityFramework.EF
{
    public class HlxBeDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<UserAvatar> UserAvatars { get; set; }



        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public HlxBeDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in HlxBeDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of HlxBeDbContext since ABP automatically handles it.
         */
        public HlxBeDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public HlxBeDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserAvatarConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
