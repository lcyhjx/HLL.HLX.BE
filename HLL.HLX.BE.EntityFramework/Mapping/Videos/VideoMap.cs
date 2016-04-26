using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.Mapping.Videos
{
    public class VideoConfiguration : HlxEntityTypeConfiguration<Video>
    {
        public VideoConfiguration()
        {
            ToTable("Video");

            Property(x => x.Title).IsRequired();
            Property(x => x.Title).HasMaxLength(200);
            Property(x => x.CoverPicPath).HasMaxLength(300);
            Property(x => x.StreamMediaPath).HasMaxLength(300);

            Property(x => x.Status).IsRequired();
            Property(x => x.PublishUserId).IsRequired();
            Property(x => x.limelightCount).IsRequired();
        }
    }
}
