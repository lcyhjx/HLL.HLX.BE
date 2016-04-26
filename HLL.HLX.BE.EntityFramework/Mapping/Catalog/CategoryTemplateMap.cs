using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class CategoryTemplateMap : HlxEntityTypeConfiguration<CategoryTemplate>
    {
        public CategoryTemplateMap()
        {
            this.ToTable("CategoryTemplate");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.ViewPath).IsRequired().HasMaxLength(400);
        }
    }
}