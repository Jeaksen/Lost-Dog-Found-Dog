using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class dogshelterrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dogs_ShelterId",
                table: "Dogs",
                column: "ShelterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_Shelters_ShelterId",
                table: "Dogs",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_Shelters_ShelterId",
                table: "Dogs");

            migrationBuilder.DropIndex(
                name: "IX_Dogs_ShelterId",
                table: "Dogs");
        }
    }
}
