using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Abp.Zero.EntityFramework;
using HLL.HLX.BE.Core.Model.Authorization.Roles;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using HLL.HLX.BE.Core.Model.Users;
using HLL.HLX.BE.Core.Model.Videos;
using HLL.HLX.BE.EntityFramework.Mapping;
using HLL.HLX.BE.EntityFramework.Mapping.Users;
using HLL.HLX.BE.EntityFramework.Mapping.Videos;

namespace HLL.HLX.BE.EntityFramework
{
    public class HlxBeDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        //public virtual IDbSet<UserAvatar> UserAvatars { get; set; }
        //public virtual IDbSet<Video> Videos { get; set; }



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

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(HlxEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());

            //modelBuilder.Configurations.Add(new UserMap());
            //modelBuilder.Configurations.Add(new UserAvatarMap());
            //modelBuilder.Configurations.Add(new VideoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
