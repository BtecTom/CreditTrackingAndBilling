using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpCredis",
                table: "Organisations");

            migrationBuilder.AddColumn<int>(
                name: "TopUpCredits",
                table: "Organisations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpCredits",
                table: "Organisations");

            migrationBuilder.AddColumn<int>(
                name: "TopUpCredis",
                table: "Organisations",
                type: "int",
                nullable: true);
        }
    }
}
