using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.EF.DbConfiguration
{
    public class VideoConfiguration : EntityTypeConfiguration<Video>
    {
        public VideoConfiguration()
        {
            ToTable("HlxVideo");

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
