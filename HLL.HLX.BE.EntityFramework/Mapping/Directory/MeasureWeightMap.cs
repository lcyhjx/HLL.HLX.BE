using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.EntityFramework.Mapping.Directory
{
    public partial class MeasureWeightMap : HlxEntityTypeConfiguration<MeasureWeight>
    {
        public MeasureWeightMap()
        {
            this.ToTable("MeasureWeight");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(100);
            this.Property(m => m.SystemKeyword).IsRequired().HasMaxLength(100);
            this.Property(m => m.Ratio).HasPrecision(18, 8);
        }
    }
}