using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using HLL.HLX.BE.Core.Business;
using HLL.HLX.BE.Core.Model;

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
        }
    }
}