using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class AddBookDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "demob_author",
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
                    name = table.Column<string>(maxLength: 120, nullable: false),
                    age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_author", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_book",
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
                    name = table.Column<string>(maxLength: 120, nullable: false),
                    price = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    author_id = table.Column<Guid>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "demob_book");

            migrationBuilder.DropTable(
                name: "demob_author");
        }
    }
}
