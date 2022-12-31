using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoC.DbMapperCfg.SearchDocuments
{
    public class ProductSpuDocCfg : IEntityTypeConfiguration<ProductSpuDoc>
    {
        public void Configure(EntityTypeBuilder<ProductSpuDoc> builder)
        {
            builder.ToTable(DemoCDbProperties.DbTablePrefix + nameof(ProductSpuDoc), DemoCDbProperties.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.DocId)
                .HasMaxLength(64)
                .HasColumnName(nameof(ProductSpuDoc.DocId));
            builder.Property(p => p.SpuCode).IsRequired()
                .HasMaxLength(24)
                .HasColumnName(nameof(ProductSpuDoc.SpuCode));
            builder.Property(p => p.SumSkuCode)
              .HasMaxLength(240)
              .HasColumnName(nameof(ProductSpuDoc.SumSkuCode));
            builder.Property(p => p.SpuName).IsRequired()
              .HasMaxLength(64)
              .HasColumnName(nameof(ProductSpuDoc.SpuName));

            builder.Property(p => p.Brand).IsRequired()
              .HasMaxLength(24)
              .HasColumnName(nameof(ProductSpuDoc.Brand));
            builder.Property(p => p.SpuKeywords).IsRequired()
              .HasMaxLength(64)
              .HasColumnName(nameof(ProductSpuDoc.SpuKeywords));
            builder.Property(p => p.SumSkuKeywords)
              .HasMaxLength(640)
              .HasColumnName(nameof(ProductSpuDoc.SumSkuKeywords));

            builder.Property(p => p.Currency)
              .HasMaxLength(8)
              .HasColumnName(nameof(ProductSpuDoc.Currency));
            builder.Property(p => p.MinPrice)
              .HasColumnType("decimal(18,6)")
              .HasColumnName(nameof(ProductSpuDoc.MinPrice));
            builder.Property(p => p.MaxPrice)
               .HasColumnType("decimal(18,6)")
              .HasColumnName(nameof(ProductSpuDoc.MaxPrice));
        }
    }
}
