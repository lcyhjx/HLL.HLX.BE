using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using HLL.HLX.BE.Application;

namespace HLL.HLX.BE.WebApi.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(HlxBeApplicationModule))]
    public class HlxBeWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //DynamicApiControllerBuilder
            //    .ForAll<IApplicationService>(typeof(HlxBeApplicationModule).Assembly, "app")
            //    .Build();

            DynamicApiControllerBuilder
              .For<Application.Users.IUserAppService>("app/user")
              .Build();

            DynamicApiControllerBuilder
              .For<Application.Sessions.ISessionAppService>("app/session")
              .Build();

            DynamicApiControllerBuilder
              .For<Application.Roles.IRoleAppService>("app/role")
              .Build();

            DynamicApiControllerBuilder
              .For<Application.MultiTenancy.ITenantAppService>("app/tenant")
              .Build();


            #region Mobility Web Api
            DynamicApiControllerBuilder
               .For<Application.Mobility.Users.IUserAppService>("app/mobility/user")
               .Build();

            DynamicApiControllerBuilder
               .For<Application.Mobility.Videos.IVideoAppService>("app/mobility/video")
               .Build();
            #endregion

            #region MobilityH5 Web Api
            DynamicApiControllerBuilder
               .For<Application.MobilityH5.Products.IProductAppService>("app/mobilityh5/product")
               .Build();
            #endregion

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}
