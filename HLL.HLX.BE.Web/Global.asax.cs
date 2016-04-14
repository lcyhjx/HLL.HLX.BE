using System;
using System.Data.Entity;
using System.Web;
using Abp.Web;
using Castle.Facilities.Logging;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Common.Infrastructure;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.EntityFramework.EF;

namespace HLL.HLX.BE.Web
{
    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);


            HlxBeContext.AppBinPath = HttpRuntime.BinDirectory;
            HlxBeContext.AppDomainPath = HttpRuntime.AppDomainAppPath;

            //initialize engine context
            EngineContext.Initialize(false);

            bool migrateDbToLatestVersionEnabled = AppSettingUtil.GetAppSetting4Bool("MigrateDbToLatestVersionEnabled");
            if (migrateDbToLatestVersionEnabled)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<HlxBeDbContext, EntityFramework.Migrations.Configuration>());
            }
        }
    }
}
