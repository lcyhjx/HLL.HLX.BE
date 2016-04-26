using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.EntityFramework.Mapping.Common
{
    public partial class GenericAttributeMap : HlxEntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");
            this.HasKey(ga => ga.Id);

            this.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Value).IsRequired();
        }
    }
}