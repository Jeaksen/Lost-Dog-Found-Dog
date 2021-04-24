using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class behaviortypofix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Behvaior",
                table: "DogBehaviors",
                newName: "Behavior");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Pictures",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Behavior",
                table: "DogBehaviors",
                newName: "Behvaior");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Pictures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
