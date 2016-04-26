using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.EntityFramework.Mapping.Shipping
{
    public partial class ShipmentItemMap : HlxEntityTypeConfiguration<ShipmentItem>
    {
        public ShipmentItemMap()
        {
            this.ToTable("ShipmentItem");
            this.HasKey(si => si.Id);

            this.HasRequired(si => si.Shipment)
                .WithMany(s => s.ShipmentItems)
                .HasForeignKey(si => si.ShipmentId);
        }
    }
}