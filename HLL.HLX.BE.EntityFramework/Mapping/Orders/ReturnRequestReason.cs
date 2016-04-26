using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Mapping.Orders
{
    public partial class ReturnRequestReasonMap : HlxEntityTypeConfiguration<ReturnRequestReason>
    {
        public ReturnRequestReasonMap()
        {
            this.ToTable("ReturnRequestReason");
            this.HasKey(rrr => rrr.Id);
            this.Property(rrr => rrr.Name).IsRequired().HasMaxLength(400);
        }
    }
}