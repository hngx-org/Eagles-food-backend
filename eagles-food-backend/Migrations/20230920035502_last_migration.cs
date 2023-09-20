using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eagles_food_backend.Migrations
{
    public partial class last_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lunches_Users_UserId",
                table: "Lunches");

            migrationBuilder.DropIndex(
                name: "IX_Lunches_UserId",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lunches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Lunches",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_UserId",
                table: "Lunches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lunches_Users_UserId",
                table: "Lunches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
