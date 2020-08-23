using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class AddSaleOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "demob_sale_order",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    order_no = table.Column<string>(maxLength: 64, nullable: false),
                    currency = table.Column<string>(maxLength: 8, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    order_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_sale_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_sale_order_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: false),
                    line_no = table.Column<int>(nullable: false),
                    product_sku_code = table.Column<string>(maxLength: 64, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_sale_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_demob_sale_order_detail_demob_sale_order_parent_id",
                        column: x => x.parent_id,
                        principalTable: "demob_sale_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_demob_sale_order_detail_parent_id",
                table: "demob_sale_order_detail",
                column: "parent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "demob_sale_order_detail");

            migrationBuilder.DropTable(
                name: "demob_sale_order");
        }
    }
}
