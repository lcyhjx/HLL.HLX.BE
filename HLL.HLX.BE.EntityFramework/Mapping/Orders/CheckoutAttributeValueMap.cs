using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Mapping.Orders
{
    public partial class CheckoutAttributeValueMap : HlxEntityTypeConfiguration<CheckoutAttributeValue>
    {
        public CheckoutAttributeValueMap()
        {
            this.ToTable("CheckoutAttributeValue");
            this.HasKey(cav => cav.Id);
            this.Property(cav => cav.Name).IsRequired().HasMaxLength(400);
            this.Property(cav => cav.ColorSquaresRgb).HasMaxLength(100);
            this.Property(cav => cav.PriceAdjustment).HasPrecision(18, 4);
            this.Property(cav => cav.WeightAdjustment).HasPrecision(18, 4);

            this.HasRequired(cav => cav.CheckoutAttribute)
                .WithMany(ca => ca.CheckoutAttributeValues)
                .HasForeignKey(cav => cav.CheckoutAttributeId);
        }
    }
}