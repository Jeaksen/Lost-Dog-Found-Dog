using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class shelterIdaddedtoaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShelterId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelterId",
                table: "AspNetUsers");
        }
    }
}
