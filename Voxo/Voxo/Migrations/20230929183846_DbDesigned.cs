using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.Migrations
{
    public partial class DbDesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentCards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CVC = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCards_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_AppUserId",
                table: "PaymentCards",
                column: "AppUserId");
        }
    }
}
