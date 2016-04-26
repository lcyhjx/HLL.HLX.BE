using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class CategoryMap : HlxEntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Category");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            this.Property(c => c.MetaKeywords).HasMaxLength(400);
            this.Property(c => c.MetaTitle).HasMaxLength(400);
            this.Property(c => c.PriceRanges).HasMaxLength(400);
            this.Property(c => c.PageSizeOptions).HasMaxLength(200);
        }
    }
}