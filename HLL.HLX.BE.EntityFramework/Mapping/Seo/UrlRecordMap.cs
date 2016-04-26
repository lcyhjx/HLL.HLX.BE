using HLL.HLX.BE.Core.Model.Seo;

namespace HLL.HLX.BE.EntityFramework.Mapping.Seo
{
    public partial class UrlRecordMap : HlxEntityTypeConfiguration<UrlRecord>
    {
        public UrlRecordMap()
        {
            this.ToTable("UrlRecord");
            this.HasKey(lp => lp.Id);

            this.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
        }
    }
}