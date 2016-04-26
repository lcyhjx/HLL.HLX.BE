using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.EntityFramework.Mapping.Shipping
{
    public partial class ShipmentMap : HlxEntityTypeConfiguration<Shipment>
    {
        public ShipmentMap()
        {
            this.ToTable("Shipment");
            this.HasKey(s => s.Id);

            this.Property(s => s.TotalWeight).HasPrecision(18, 4);
            
            this.HasRequired(s => s.Order)
                .WithMany(o => o.Shipments)
                .HasForeignKey(s => s.OrderId);
        }
    }
}