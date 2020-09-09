using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoC.Migrations
{
    public partial class UpdateProductSpuDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_democ_product_spu_doc",
                table: "democ_product_spu_doc");

            migrationBuilder.RenameTable(
                name: "democ_product_spu_doc",
                newName: "product_spu_doc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_spu_doc",
                table: "product_spu_doc",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_product_spu_doc",
                table: "product_spu_doc");

            migrationBuilder.RenameTable(
                name: "product_spu_doc",
                newName: "democ_product_spu_doc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_democ_product_spu_doc",
                table: "democ_product_spu_doc",
                column: "id");
        }
    }
}
