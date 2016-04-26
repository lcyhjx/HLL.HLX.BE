using System.Data.Entity.ModelConfiguration;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.Mapping.Users
{
    public partial class UserAvatarMap : HlxEntityTypeConfiguration<UserAvatar>
    {
        public UserAvatarMap()
        {
            ToTable("UserAvatar");
            Property(x => x.ImageFilePath).HasMaxLength(200);
            Property(x => x.Name).HasMaxLength(100);
            Property(x => x.Description).HasMaxLength(500);
        }
    }
}
