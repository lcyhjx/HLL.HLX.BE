using HLL.HLX.BE.Core.Model.Media;

namespace HLL.HLX.BE.EntityFramework.Mapping.Media
{
    public partial class DownloadMap : HlxEntityTypeConfiguration<Download>
    {
        public DownloadMap()
        {
            this.ToTable("Download");
            this.HasKey(p => p.Id);
            this.Property(p => p.DownloadBinary).IsMaxLength();
        }
    }
}