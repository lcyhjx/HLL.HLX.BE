using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.EntityFramework.Mapping.Users
{
    public partial class RewardPointsHistoryMap : HlxEntityTypeConfiguration<RewardPointsHistory>
    {
        public RewardPointsHistoryMap()
        {
            this.ToTable("RewardPointsHistory");
            this.HasKey(rph => rph.Id);

            this.Property(rph => rph.UsedAmount).HasPrecision(18, 4);

            this.HasRequired(rph => rph.Customer)
                .WithMany()
                .HasForeignKey(rph => rph.CustomerId);

            this.HasOptional(rph => rph.UsedWithOrder)
                .WithOptionalDependent(o => o.RedeemedRewardPointsEntry)
                .WillCascadeOnDelete(false);
        }
    }
}