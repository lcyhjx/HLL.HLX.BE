using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Mapping.Directory
{
    public partial class StateProvinceMap : HlxEntityTypeConfiguration<StateProvince>
    {
        public StateProvinceMap()
        {
            this.ToTable("StateProvince");
            this.HasKey(sp => sp.Id);
            this.Property(sp => sp.Name).IsRequired().HasMaxLength(100);
            this.Property(sp => sp.Abbreviation).HasMaxLength(100);


            this.HasRequired(sp => sp.Country)
                .WithMany(c => c.StateProvinces)
                .HasForeignKey(sp => sp.CountryId);
        }
    }
}