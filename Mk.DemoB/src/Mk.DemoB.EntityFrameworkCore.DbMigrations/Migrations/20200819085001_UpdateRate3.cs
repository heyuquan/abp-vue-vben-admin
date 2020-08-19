using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class UpdateRate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "capture_time",
                table: "demob_exchange_rate_capture_batch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "capture_time",
                table: "demob_exchange_rate_capture_batch");
        }
    }
}
