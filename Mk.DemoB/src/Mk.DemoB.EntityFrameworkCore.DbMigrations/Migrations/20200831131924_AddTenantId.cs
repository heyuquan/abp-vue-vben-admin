using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class AddTenantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "demob_sale_order_detail",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "demob_sale_order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "demob_sale_order_detail");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "demob_sale_order");
        }
    }
}
