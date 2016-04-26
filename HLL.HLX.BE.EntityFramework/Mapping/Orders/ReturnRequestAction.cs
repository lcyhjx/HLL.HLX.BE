using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Mapping.Orders
{
    public partial class ReturnRequestActionMap : HlxEntityTypeConfiguration<ReturnRequestAction>
    {
        public ReturnRequestActionMap()
        {
            this.ToTable("ReturnRequestAction");
            this.HasKey(rra => rra.Id);
            this.Property(rra => rra.Name).IsRequired().HasMaxLength(400);
        }
    }
}