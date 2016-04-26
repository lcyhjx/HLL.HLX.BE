using HLL.HLX.BE.Core.Model.Vendors;

namespace HLL.HLX.BE.EntityFramework.Mapping.Vendors
{
    public partial class VendorNoteMap : HlxEntityTypeConfiguration<VendorNote>
    {
        public VendorNoteMap()
        {
            this.ToTable("VendorNote");
            this.HasKey(vn => vn.Id);
            this.Property(vn => vn.Note).IsRequired();

            this.HasRequired(vn => vn.Vendor)
                .WithMany(v => v.VendorNotes)
                .HasForeignKey(vn => vn.VendorId);
        }
    }
}