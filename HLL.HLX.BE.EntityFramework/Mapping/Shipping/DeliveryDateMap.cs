using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.EntityFramework.Mapping.Shipping
{
    public class DeliveryDateMap : HlxEntityTypeConfiguration<DeliveryDate>
    {
        public DeliveryDateMap()
        {
            this.ToTable("DeliveryDate");
            this.HasKey(dd => dd.Id);
            this.Property(dd => dd.Name).IsRequired().HasMaxLength(400);
        }
    }
}
