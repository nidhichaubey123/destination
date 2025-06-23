using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateTruvalQueriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TruvaiQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HandledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaxCount = table.Column<int>(type: "int", nullable: true),
                    GitFit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversionProbability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TravelPlans = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhyLost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastReplied = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalPax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostSheetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndClient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationLead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderOverdue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruvaiQueries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruvaiQueries");
        }
    }
}
