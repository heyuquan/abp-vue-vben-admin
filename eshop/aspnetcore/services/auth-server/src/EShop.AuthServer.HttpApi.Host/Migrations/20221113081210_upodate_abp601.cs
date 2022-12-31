using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.AuthServer.HttpApi.Host.Migrations
{
    public partial class upodate_abp601 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpUsers");
        }
    }
}
