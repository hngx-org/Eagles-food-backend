using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eagles_food_backend.Migrations
{
    public partial class OrganizationUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Organizations_OrganizationId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Lunches_Organizations_recieverId",
                table: "Lunches");

            migrationBuilder.DropForeignKey(
                name: "FK_Lunches_Organizations_senderId",
                table: "Lunches");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationWallets_Organizations_OrganizationId",
                table: "OrganizationWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_Users_UserId",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lunches",
                table: "Lunches");

            migrationBuilder.DropIndex(
                name: "IX_Lunches_recieverId",
                table: "Lunches");

            migrationBuilder.DropIndex(
                name: "IX_Lunches_senderId",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "WithdrawalId",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LunchId",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "recieverId",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "senderId",
                table: "Lunches");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Withdrawals",
                newName: "Userid");

            migrationBuilder.RenameColumn(
                name: "ammount",
                table: "Withdrawals",
                newName: "amount");

            migrationBuilder.RenameIndex(
                name: "IX_Withdrawals_UserId",
                table: "Withdrawals",
                newName: "IX_Withdrawals_Userid");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Users",
                newName: "organizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                newName: "IX_Users_organizationId");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "OrganizationWallets",
                newName: "Balance");

            migrationBuilder.AlterColumn<int>(
                name: "Userid",
                table: "Withdrawals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Withdrawals",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "Withdrawals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "organizationId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "OrganizationWallets",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Lunches",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Organizationid",
                table: "Lunches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Organizationid1",
                table: "Lunches",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Invites",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lunches",
                table: "Lunches",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_Organizationid",
                table: "Lunches",
                column: "Organizationid");

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_Organizationid1",
                table: "Lunches",
                column: "Organizationid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Organizations_OrganizationId",
                table: "Invites",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lunches_Organizations_Organizationid",
                table: "Lunches",
                column: "Organizationid",
                principalTable: "Organizations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lunches_Organizations_Organizationid1",
                table: "Lunches",
                column: "Organizationid1",
                principalTable: "Organizations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationWallets_Organizations_OrganizationId",
                table: "OrganizationWallets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_organizationId",
                table: "Users",
                column: "organizationId",
                principalTable: "Organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawals_Users_Userid",
                table: "Withdrawals",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Organizations_OrganizationId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Lunches_Organizations_Organizationid",
                table: "Lunches");

            migrationBuilder.DropForeignKey(
                name: "FK_Lunches_Organizations_Organizationid1",
                table: "Lunches");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationWallets_Organizations_OrganizationId",
                table: "OrganizationWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_organizationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_Users_Userid",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lunches",
                table: "Lunches");

            migrationBuilder.DropIndex(
                name: "IX_Lunches_Organizationid",
                table: "Lunches");

            migrationBuilder.DropIndex(
                name: "IX_Lunches_Organizationid1",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "Organizationid",
                table: "Lunches");

            migrationBuilder.DropColumn(
                name: "Organizationid1",
                table: "Lunches");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Withdrawals",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Withdrawals",
                newName: "ammount");

            migrationBuilder.RenameIndex(
                name: "IX_Withdrawals_Userid",
                table: "Withdrawals",
                newName: "IX_Withdrawals_UserId");

            migrationBuilder.RenameColumn(
                name: "organizationId",
                table: "Users",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_organizationId",
                table: "Users",
                newName: "IX_Users_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "OrganizationWallets",
                newName: "balance");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Withdrawals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "WithdrawalId",
                table: "Withdrawals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizationId",
                table: "Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizationId",
                table: "OrganizationWallets",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "Organizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "LunchId",
                table: "Lunches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "recieverId",
                table: "Lunches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "senderId",
                table: "Lunches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizationId",
                table: "Invites",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals",
                column: "WithdrawalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lunches",
                table: "Lunches",
                column: "LunchId");

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_recieverId",
                table: "Lunches",
                column: "recieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_senderId",
                table: "Lunches",
                column: "senderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Organizations_OrganizationId",
                table: "Invites",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lunches_Organizations_recieverId",
                table: "Lunches",
                column: "recieverId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lunches_Organizations_senderId",
                table: "Lunches",
                column: "senderId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationWallets_Organizations_OrganizationId",
                table: "OrganizationWallets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawals_Users_UserId",
                table: "Withdrawals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
