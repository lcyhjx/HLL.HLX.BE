using System.CodeDom;
using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using HLL.HLX.BE.Core.Business.Authorization;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Core.Business.Vendors;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Authorization;
using Castle.MicroKernel.Registration;
using HLL.HLX.BE.Core.Business.Catalog;
using HLL.HLX.BE.Core.Business.Orders;
using HLL.HLX.BE.Core.Business.Stores;

namespace HLL.HLX.BE.Core.Business
{
    [DependsOn(typeof(AbpZeroCoreModule),typeof(HlxBeCoreModelModule))]
    public class HlxBeCoreBusinessModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Remove the following line to disable multi-tenancy.
            //Configuration.MultiTenancy.IsEnabled = true;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    HlxBeConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "HLL.HLX.BE.Core.Business.Localization.Source"
                        )
                    )
                );

            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<HlxBeAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(Component.For<IVendorTest>().ImplementedBy<VendorTest>().LifestyleTransient());
            IocManager.IocContainer.Register(Component.For<IStoreContext>().ImplementedBy<WebStoreContext>().LifestylePerWebRequest());
            //IocManager.IocContainer.Register(Component.For<IWorkContext>().ImplementedBy<WebWorkContext>().LifestylePerWebRequest());
            IocManager.IocContainer.Register(Component.For<IProductAttributeParser>().ImplementedBy<ProductAttributeParser>().LifestylePerWebRequest());
            IocManager.IocContainer.Register(Component.For<IPriceFormatter>().ImplementedBy<PriceFormatter>().LifestylePerWebRequest());
            IocManager.IocContainer.Register(Component.For<ICheckoutAttributeParser>().ImplementedBy<CheckoutAttributeParser>().LifestylePerWebRequest());
        }
    }
}
