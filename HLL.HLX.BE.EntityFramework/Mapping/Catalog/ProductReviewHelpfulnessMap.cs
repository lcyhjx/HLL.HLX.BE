using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class ProductReviewHelpfulnessMap : HlxEntityTypeConfiguration<ProductReviewHelpfulness>
    {
        public ProductReviewHelpfulnessMap()
        {
            this.ToTable("ProductReviewHelpfulness");
            this.HasKey(pr => pr.Id);

            this.HasRequired(prh => prh.ProductReview)
                .WithMany(pr => pr.ProductReviewHelpfulnessEntries)
                .HasForeignKey(prh => prh.ProductReviewId).WillCascadeOnDelete(true);
        }
    }
}