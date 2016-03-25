using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.EF.DbConfiguration
{
    public class UserAvatarConfiguration : EntityTypeConfiguration<UserAvatar>
    {
        public UserAvatarConfiguration()
        {
            ToTable("HlxUserAvatar");
            Property(x => x.ImageFilePath).HasMaxLength(200);
            Property(x => x.Name).HasMaxLength(100);
            Property(x => x.Description).HasMaxLength(500);
        }
    }
}
