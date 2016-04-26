using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.EntityFramework.Mapping.Orders
{
    public partial class OrderNoteMap : HlxEntityTypeConfiguration<OrderNote>
    {
        public OrderNoteMap()
        {
            this.ToTable("OrderNote");
            this.HasKey(on => on.Id);
            this.Property(on => on.Note).IsRequired();

            this.HasRequired(on => on.Order)
                .WithMany(o => o.OrderNotes)
                .HasForeignKey(on => on.OrderId);
        }
    }
}