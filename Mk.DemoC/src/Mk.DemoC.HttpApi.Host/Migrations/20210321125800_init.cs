using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoC.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_spu_doc",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    doc_id = table.Column<string>(maxLength: 64, nullable: true),
                    spu_code = table.Column<string>(maxLength: 24, nullable: false),
                    sum_sku_code = table.Column<string>(maxLength: 240, nullable: true),
                    spu_name = table.Column<string>(maxLength: 64, nullable: false),
                    brand = table.Column<string>(maxLength: 24, nullable: false),
                    spu_keywords = table.Column<string>(maxLength: 64, nullable: false),
                    sum_sku_keywords = table.Column<string>(maxLength: 640, nullable: true),
                    currency = table.Column<string>(maxLength: 8, nullable: true),
                    min_price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    max_price = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_spu_doc", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_spu_doc");
        }
    }
}
