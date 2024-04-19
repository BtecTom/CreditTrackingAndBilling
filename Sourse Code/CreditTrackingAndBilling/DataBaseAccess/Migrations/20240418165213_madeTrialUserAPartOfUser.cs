using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class madeTrialUserAPartOfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportRequests_Users_UserId",
                table: "ReportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TrialUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AbstractUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_OrganisationId",
                table: "AbstractUser",
                newName: "IX_AbstractUser_OrganisationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "AbstractUser",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbstractUser",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstReportRanDate",
                table: "AbstractUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrialCompleted",
                table: "AbstractUser",
                type: "bit",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbstractUser");

            migrationBuilder.DropColumn(
                name: "FirstReportRanDate",
                table: "AbstractUser");

            migrationBuilder.DropColumn(
                name: "TrialCompleted",
                table: "AbstractUser");

            migrationBuilder.RenameTable(
                name: "AbstractUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_AbstractUser_OrganisationId",
                table: "Users",
                newName: "IX_Users_OrganisationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "TrialUsers",
                columns: table => new
                {
                    TrialUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditsUsed = table.Column<int>(type: "int", nullable: false),
                    FirstReportRanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrialCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrialUsers", x => x.TrialUserID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportRequests_Users_UserId",
                table: "ReportRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "OrganisationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
