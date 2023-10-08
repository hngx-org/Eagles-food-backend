using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eagles_food_backend.Migrations
{
    public partial class AddInviteRequestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "org_id3",
                table: "users",
                newName: "org_id4");

            migrationBuilder.RenameIndex(
                name: "org_id2",
                table: "organization_lunch_wallets",
                newName: "org_id3");

            migrationBuilder.RenameIndex(
                name: "org_id1",
                table: "organization_invites",
                newName: "org_id2");

            migrationBuilder.RenameIndex(
                name: "org_id",
                table: "lunches",
                newName: "org_id1");

            migrationBuilder.AddColumn<bool>(
                name: "InviteFromOrganization",
                table: "organization_invites",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "invitation_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    token = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    org_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invitation_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_invitation_request_organizations_org_id",
                        column: x => x.org_id,
                        principalTable: "organizations",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "org_id",
                table: "invitation_request",
                column: "org_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invitation_request");

            migrationBuilder.DropColumn(
                name: "InviteFromOrganization",
                table: "organization_invites");

            migrationBuilder.RenameIndex(
                name: "org_id4",
                table: "users",
                newName: "org_id3");

            migrationBuilder.RenameIndex(
                name: "org_id3",
                table: "organization_lunch_wallets",
                newName: "org_id2");

            migrationBuilder.RenameIndex(
                name: "org_id2",
                table: "organization_invites",
                newName: "org_id1");

            migrationBuilder.RenameIndex(
                name: "org_id1",
                table: "lunches",
                newName: "org_id");
        }
    }
}
