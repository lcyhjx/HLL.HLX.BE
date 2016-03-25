using System.Data.Entity.ModelConfiguration;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.EF.DbConfiguration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(x => x.NickName).HasMaxLength(50);
            Property(x => x.Gender).HasMaxLength(10);
            Property(x => x.Company).HasMaxLength(200);
            Property(x => x.Title).HasMaxLength(100);
            Property(x => x.PhoneNumber).HasMaxLength(50);              
        }
    }
}
