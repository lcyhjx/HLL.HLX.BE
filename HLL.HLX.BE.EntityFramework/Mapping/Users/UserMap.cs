using System.Data.Entity.ModelConfiguration;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.Mapping.Users
{
    public partial class UserMap : HlxEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            Property(x => x.NickName).HasMaxLength(50);
            Property(x => x.Gender).HasMaxLength(10);
            Property(x => x.Company).HasMaxLength(200);
            Property(x => x.Title).HasMaxLength(100);
            Property(x => x.PhoneNumber).HasMaxLength(50);

            this.HasMany(c => c.Addresses)
               .WithMany()
               .Map(m => m.ToTable("CustomerAddresses"));

            this.HasOptional(c => c.BillingAddress);
            this.HasOptional(c => c.ShippingAddress);
        }
    }
}
