using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToTruvalQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Budget",
                table: "TruvaiQueries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TruvaiQueries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TruvaiQueries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "TruvaiQueries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "TruvaiQueries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "TruvaiQueries");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TruvaiQueries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TruvaiQueries");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "TruvaiQueries");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "TruvaiQueries");
        }
    }
}
