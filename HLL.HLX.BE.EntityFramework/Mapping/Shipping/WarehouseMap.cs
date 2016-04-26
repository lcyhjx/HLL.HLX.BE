using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.EntityFramework.Mapping.Shipping
{
    public class WarehouseMap : HlxEntityTypeConfiguration<Warehouse>
    {
        public WarehouseMap()
        {
            this.ToTable("Warehouse");
            this.HasKey(wh => wh.Id);
            this.Property(wh => wh.Name).IsRequired().HasMaxLength(400);
        }
    }
}
