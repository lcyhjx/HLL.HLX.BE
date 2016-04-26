using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class SpecificationAttributeMap : HlxEntityTypeConfiguration<SpecificationAttribute>
    {
        public SpecificationAttributeMap()
        {
            this.ToTable("SpecificationAttribute");
            this.HasKey(sa => sa.Id);
            this.Property(sa => sa.Name).IsRequired();
        }
    }
}