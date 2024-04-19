using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractUser_Organisations_OrganisationId",
                table: "AbstractUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportRequests_AbstractUser_UserId",
                table: "ReportRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbstractUser",
                table: "AbstractUser");

            migrationBuilder.RenameTable(
                name: "AbstractUser",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_AbstractUser_OrganisationId",
                table: "User",
                newName: "IX_User_OrganisationId");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "User",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportRequests_User_UserId",
                table: "ReportRequests",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organisations_OrganisationId",
                table: "User",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "OrganisationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportRequests_User_UserId",
                table: "ReportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Organisations_OrganisationId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "AbstractUser");

            migrationBuilder.RenameIndex(
                name: "IX_User_OrganisationId",
                table: "AbstractUser",
                newName: "IX_AbstractUser_OrganisationId");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AbstractUser",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbstractUser",
                table: "AbstractUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractUser_Organisations_OrganisationId",
                table: "AbstractUser",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "OrganisationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportRequests_AbstractUser_UserId",
                table: "ReportRequests",
                column: "UserId",
                principalTable: "AbstractUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
