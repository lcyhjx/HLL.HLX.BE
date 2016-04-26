using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Mapping.Directory
{
    public partial class CountryMap : HlxEntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            this.ToTable("Country");
            this.HasKey(c =>c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(100);
            this.Property(c =>c.TwoLetterIsoCode).HasMaxLength(2);
            this.Property(c =>c.ThreeLetterIsoCode).HasMaxLength(3);
        }
    }
}