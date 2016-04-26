using HLL.HLX.BE.Core.Model.Discounts;

namespace HLL.HLX.BE.EntityFramework.Mapping.Discounts
{
    public partial class DiscountRequirementMap : HlxEntityTypeConfiguration<DiscountRequirement>
    {
        public DiscountRequirementMap()
        {
            this.ToTable("DiscountRequirement");
            this.HasKey(dr => dr.Id);
        }
    }
}