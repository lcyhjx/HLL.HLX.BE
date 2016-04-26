using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class ManufacturerTemplateMap : HlxEntityTypeConfiguration<ManufacturerTemplate>
    {
        public ManufacturerTemplateMap()
        {
            this.ToTable("ManufacturerTemplate");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.ViewPath).IsRequired().HasMaxLength(400);
        }
    }
}