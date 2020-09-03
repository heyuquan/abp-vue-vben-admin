using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class deleteBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "demob_book");

            migrationBuilder.DropTable(
                name: "demob_author");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "demob_author",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    creator_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    deleter_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    extra_properties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    last_modification_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    name = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_author", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_book",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    author_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    creator_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    deleter_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    extra_properties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    last_modification_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    name = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_book", x => x.id);
                    table.ForeignKey(
                        name: "FK_demob_book_demob_author_author_id",
                        column: x => x.author_id,
                        principalTable: "demob_author",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_demob_book_author_id",
                table: "demob_book",
                column: "author_id");
        }
    }
}
