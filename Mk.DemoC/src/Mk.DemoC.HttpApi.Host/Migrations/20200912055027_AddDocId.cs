using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoC.Migrations
{
    public partial class AddDocId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "doc_id",
                table: "product_spu_doc",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "doc_id",
                table: "product_spu_doc");
        }
    }
}
