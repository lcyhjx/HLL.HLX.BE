using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.EntityFramework.Mapping.Stores
{
    public partial class StoreMappingMap : HlxEntityTypeConfiguration<StoreMapping>
    {
        public StoreMappingMap()
        {
            this.ToTable("StoreMapping");
            this.HasKey(sm => sm.Id);

            this.Property(sm => sm.EntityName).IsRequired().HasMaxLength(400);

            this.HasRequired(sm => sm.Store)
                .WithMany()
                .HasForeignKey(sm => sm.StoreId)
                .WillCascadeOnDelete(true);
        }
    }
}