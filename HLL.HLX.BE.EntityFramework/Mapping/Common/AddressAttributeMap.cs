using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.EntityFramework.Mapping.Common
{
    public partial class AddressAttributeMap : HlxEntityTypeConfiguration<AddressAttribute>
    {
        public AddressAttributeMap()
        {
            this.ToTable("AddressAttribute");
            this.HasKey(aa => aa.Id);
            this.Property(aa => aa.Name).IsRequired().HasMaxLength(400);

            this.Ignore(aa => aa.AttributeControlType);
        }
    }
}