using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoC.Migrations
{
    public partial class init_democ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductSpuDoc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    DocId = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    SpuCode = table.Column<string>(type: "varchar(24) CHARACTER SET utf8mb4", maxLength: 24, nullable: false),
                    SumSkuCode = table.Column<string>(type: "varchar(240) CHARACTER SET utf8mb4", maxLength: 240, nullable: true),
                    SpuName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false),
                    Brand = table.Column<string>(type: "varchar(24) CHARACTER SET utf8mb4", maxLength: 24, nullable: false),
                    SpuKeywords = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false),
                    SumSkuKeywords = table.Column<string>(type: "varchar(640) CHARACTER SET utf8mb4", maxLength: 640, nullable: true),
                    Currency = table.Column<string>(type: "varchar(8) CHARACTER SET utf8mb4", maxLength: 8, nullable: true),
                    MinPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpuDoc", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSpuDoc");
        }
    }
}
