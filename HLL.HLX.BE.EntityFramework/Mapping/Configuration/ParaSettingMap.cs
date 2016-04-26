using HLL.HLX.BE.Core.Model.Configuration;

namespace HLL.HLX.BE.EntityFramework.Mapping.Configuration
{
    public partial class ParaSettingMap : HlxEntityTypeConfiguration<ParaSetting>
    {
        public ParaSettingMap()
        {
            this.ToTable("ParaSetting");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        }
    }
}