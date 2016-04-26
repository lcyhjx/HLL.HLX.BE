using HLL.HLX.BE.Core.Model.Media;

namespace HLL.HLX.BE.EntityFramework.Mapping.Media
{
    public partial class PictureMap : HlxEntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("Picture");
            this.HasKey(p => p.Id);
            this.Property(p => p.PictureBinary).IsMaxLength();
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
            this.Property(p => p.SeoFilename).HasMaxLength(300);
        }
    }
}