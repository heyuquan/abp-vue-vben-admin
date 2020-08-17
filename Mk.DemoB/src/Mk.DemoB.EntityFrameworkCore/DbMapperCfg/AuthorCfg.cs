using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.AuthorMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg
{
    public class AuthorCfg : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(Author), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.Name).IsRequired()
                .HasMaxLength(120)
                .HasColumnName(nameof(Author.Name));
            builder.Property(p => p.Age)
                .HasColumnName(nameof(Author.Age));
        }
    }
}
