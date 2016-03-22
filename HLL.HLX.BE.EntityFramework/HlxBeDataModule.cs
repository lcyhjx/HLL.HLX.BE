using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using HLL.HLX.BE.Core.Model;

namespace HLL.HLX.BE.EntityFramework
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(HlxBeCoreModelModule))]
    public class HlxBeDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
