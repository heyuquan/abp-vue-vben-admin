using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class AddRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "demob_book",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(120) CHARACTER SET utf8mb4",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "demob_author",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(120) CHARACTER SET utf8mb4",
                oldMaxLength: 120);

            migrationBuilder.CreateTable(
                name: "demob_capture_currency",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    currency_code_from = table.Column<string>(maxLength: 8, nullable: false),
                    currency_code_to = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_capture_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_exchange_rate",
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
                    currency_code_from = table.Column<string>(maxLength: 8, nullable: false),
                    currency_code_to = table.Column<string>(maxLength: 8, nullable: false),
                    buy_price = table.Column<decimal>(nullable: false),
                    data_from_url = table.Column<string>(maxLength: 256, nullable: false),
                    capture_batch_number = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_exchange_rate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_exchange_rate_capture_batch",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    capture_batch_number = table.Column<string>(maxLength: 64, nullable: false),
                    is_success = table.Column<bool>(nullable: false),
                    remark = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_exchange_rate_capture_batch", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "demob_capture_currency");

            migrationBuilder.DropTable(
                name: "demob_exchange_rate");

            migrationBuilder.DropTable(
                name: "demob_exchange_rate_capture_batch");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "demob_book",
                type: "varchar(120) CHARACTER SET utf8mb4",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "demob_author",
                type: "varchar(120) CHARACTER SET utf8mb4",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
