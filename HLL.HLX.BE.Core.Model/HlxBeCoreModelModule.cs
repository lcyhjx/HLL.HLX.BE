using System.Reflection;
using Abp.Modules;
using Abp.Zero;

namespace HLL.HLX.BE.Core.Model
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class HlxBeCoreModelModule : AbpModule
    {

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
