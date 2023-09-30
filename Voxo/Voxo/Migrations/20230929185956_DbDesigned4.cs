using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.Migrations
{
    public partial class DbDesigned4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Orders",
                newName: "FullAdress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullAdress",
                table: "Orders",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
