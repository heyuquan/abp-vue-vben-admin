using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mk.DemoB.BookMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoB.DbMapperCfg
{
    // 配置Has***和With***，应该把关系配置到ForeignKey所在的实体上

    public class BookCfg : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(DemoBConsts.DbTablePrefix + nameof(Book), DemoBConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(p => p.Name).IsRequired()
                .HasMaxLength(120)
                .HasColumnName(nameof(Book.Name));
            builder.Property(p => p.Price).IsRequired()
                .HasDefaultValue(0)
                .HasColumnName(nameof(Book.Price));

            builder
                .HasOne(p => p.Author)
                .WithMany(p=>p.Books)
                .HasForeignKey(x=>x.AuthorId)
                .IsRequired();
        }
    }
}
