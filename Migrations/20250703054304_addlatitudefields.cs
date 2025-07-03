using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class addlatitudefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EntryLatitude",
                table: "SalesVisits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EntryLongitude",
                table: "SalesVisits",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryLatitude",
                table: "SalesVisits");

            migrationBuilder.DropColumn(
                name: "EntryLongitude",
                table: "SalesVisits");
        }
    }
}
