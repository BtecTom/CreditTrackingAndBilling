using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Credits = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditsPerUser = table.Column<long>(type: "bigint", nullable: false),
                    CreditsUsed = table.Column<long>(type: "bigint", nullable: false),
                    TimeOfLastReportRan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TopUpCredits = table.Column<long>(type: "bigint", nullable: false),
                    TimeOfLastTopUp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.OrganisationId);
                    table.ForeignKey(
                        name: "FK_Organisations_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditsUsed = table.Column<long>(type: "bigint", nullable: false),
                    TimeOfLastReportRan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstReportRanDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrialCompleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportRequests",
                columns: table => new
                {
                    ReportRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ReportRequestSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportRequests", x => x.ReportRequestId);
                    table.ForeignKey(
                        name: "FK_ReportRequests_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_PlanId",
                table: "Organisations",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportRequests_UserId",
                table: "ReportRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganisationId",
                table: "User",
                column: "OrganisationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportRequests");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
