using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.Migrations
{
    public partial class DbDesigned5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "AspNetUsers");
        }
    }
}
