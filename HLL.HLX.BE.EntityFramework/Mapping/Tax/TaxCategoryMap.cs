using HLL.HLX.BE.Core.Model.Tax;

namespace HLL.HLX.BE.EntityFramework.Mapping.Tax
{
    public class TaxCategoryMap : HlxEntityTypeConfiguration<TaxCategory>
    {
        public TaxCategoryMap()
        {
            this.ToTable("TaxCategory");
            this.HasKey(tc => tc.Id);
            this.Property(tc => tc.Name).IsRequired().HasMaxLength(400);
        }
    }
}
