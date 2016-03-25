﻿using System.CodeDom;
using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using HLL.HLX.BE.Core.Business.Authorization;
using HLL.HLX.BE.Core.Business.Authorization.Roles;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Authorization;

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
        }
    }
}