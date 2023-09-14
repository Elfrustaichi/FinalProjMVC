using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.Migrations
{
    public partial class BannerRedesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLarge",
                table: "Banners");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Banners");

            migrationBuilder.AddColumn<bool>(
                name: "IsLarge",
                table: "Banners",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
