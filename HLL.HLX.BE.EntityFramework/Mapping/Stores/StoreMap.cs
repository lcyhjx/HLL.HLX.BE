using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.EntityFramework.Mapping.Stores
{
    public partial class StoreMap : HlxEntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            this.ToTable("Store");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(400);
            this.Property(s => s.Url).IsRequired().HasMaxLength(400);
            this.Property(s => s.SecureUrl).HasMaxLength(400);
            this.Property(s => s.Hosts).HasMaxLength(1000);

            this.Property(s => s.CompanyName).HasMaxLength(1000);
            this.Property(s => s.CompanyAddress).HasMaxLength(1000);
            this.Property(s => s.CompanyPhoneNumber).HasMaxLength(1000);
            this.Property(s => s.CompanyVat).HasMaxLength(1000);
        }
    }
}