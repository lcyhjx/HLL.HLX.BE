using System.Data.Entity.ModelConfiguration;

namespace HLL.HLX.BE.EntityFramework.Mapping
{
    public abstract class HlxEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected HlxEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}