using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.EntityFramework.Mapping.Catalog
{
    public partial class ProductAttributeValueMap : HlxEntityTypeConfiguration<ProductAttributeValue>
    {
        public ProductAttributeValueMap()
        {
            this.ToTable("ProductAttributeValue");
            this.HasKey(pav => pav.Id);
            this.Property(pav => pav.Name).IsRequired().HasMaxLength(400);
            this.Property(pav => pav.ColorSquaresRgb).HasMaxLength(100);

            this.Property(pav => pav.PriceAdjustment).HasPrecision(18, 4);
            this.Property(pav => pav.WeightAdjustment).HasPrecision(18, 4);
            this.Property(pav => pav.Cost).HasPrecision(18, 4);

            this.Ignore(pav => pav.AttributeValueType);

            this.HasRequired(pav => pav.ProductAttributeMapping)
                .WithMany(pam => pam.ProductAttributeValues)
                .HasForeignKey(pav => pav.ProductAttributeMappingId);
        }
    }
}