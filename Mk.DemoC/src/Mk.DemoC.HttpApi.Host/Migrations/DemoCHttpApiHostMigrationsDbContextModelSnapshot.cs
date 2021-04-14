﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mk.DemoC.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.Migrations
{
    [DbContext(typeof(DemoCHttpApiHostMigrationsDbContext))]
    partial class DemoCHttpApiHostMigrationsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.MySql)
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Mk.DemoC.SearchDocumentMgr.Entities.ProductSpuDoc", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24) CHARACTER SET utf8mb4")
                        .HasColumnName("Brand");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40) CHARACTER SET utf8mb4")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("char(36)")
                        .HasColumnName("CreatorId");

                    b.Property<string>("Currency")
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasColumnName("Currency");

                    b.Property<string>("DocId")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasColumnName("DocId");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("ExtraProperties");

                    b.Property<decimal>("MaxPrice")
                        .HasColumnType("decimal(18,6)")
                        .HasColumnName("MaxPrice");

                    b.Property<decimal>("MinPrice")
                        .HasColumnType("decimal(18,6)")
                        .HasColumnName("MinPrice");

                    b.Property<string>("SpuCode")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24) CHARACTER SET utf8mb4")
                        .HasColumnName("SpuCode");

                    b.Property<string>("SpuKeywords")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasColumnName("SpuKeywords");

                    b.Property<string>("SpuName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasColumnName("SpuName");

                    b.Property<string>("SumSkuCode")
                        .HasMaxLength(240)
                        .HasColumnType("varchar(240) CHARACTER SET utf8mb4")
                        .HasColumnName("SumSkuCode");

                    b.Property<string>("SumSkuKeywords")
                        .HasMaxLength(640)
                        .HasColumnType("varchar(640) CHARACTER SET utf8mb4")
                        .HasColumnName("SumSkuKeywords");

                    b.HasKey("Id");

                    b.ToTable("ProductSpuDoc");
                });
#pragma warning restore 612, 618
        }
    }
}
