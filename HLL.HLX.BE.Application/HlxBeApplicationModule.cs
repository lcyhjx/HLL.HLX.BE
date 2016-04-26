using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using HLL.HLX.BE.Application.MobilityH5.Products;
using HLL.HLX.BE.Core.Business;
using HLL.HLX.BE.Core.Model;
using Castle.MicroKernel.Registration;
using HLL.HLX.BE.Core.Business.Vendors;

namespace HLL.HLX.BE.Application
{
    [DependsOn(typeof (HlxBeCoreBusinessModule),
        typeof (HlxBeCoreModelModule),
        typeof (AbpAutoMapperModule))]
    public class HlxBeApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            //LifestyleTransient - 每次请求都创建一个实例
            //LifestyleSingleton - 单例模式
            //IocManager.IocContainer.Register(Component.For<IVendorTest>().ImplementedBy<VendorTest>().LifestyleTransient());
            //IocManager.IocContainer.Register(Component.For<IVendorTest>().Named("vendorTestIml").ImplementedBy<VendorTest>().LifestyleTransient());
        }
    }
}