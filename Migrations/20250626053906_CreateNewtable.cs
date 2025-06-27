using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionTypes",
                columns: table => new
                {
                    DiscussionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscussionTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiscussionTypeDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionTypes", x => x.DiscussionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MeetingTypes",
                columns: table => new
                {
                    MeetingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MeetingTypeDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTypes", x => x.MeetingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SalesVisits",
                columns: table => new
                {
                    SalesVisitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    DiscussionTypeId = table.Column<int>(type: "int", nullable: true),
                    MeetingTypeId = table.Column<int>(type: "int", nullable: true),
                    MeetingVenueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MeetingLatitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MeetingLongitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MeetingNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesVisitCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesVisits", x => x.SalesVisitId);
                    table.ForeignKey(
                        name: "FK_SalesVisits_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK_SalesVisits_DiscussionTypes_DiscussionTypeId",
                        column: x => x.DiscussionTypeId,
                        principalTable: "DiscussionTypes",
                        principalColumn: "DiscussionTypeId");
                    table.ForeignKey(
                        name: "FK_SalesVisits_MeetingTypes_MeetingTypeId",
                        column: x => x.MeetingTypeId,
                        principalTable: "MeetingTypes",
                        principalColumn: "MeetingTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_AgentId",
                table: "SalesVisits",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_DiscussionTypeId",
                table: "SalesVisits",
                column: "DiscussionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_MeetingTypeId",
                table: "SalesVisits",
                column: "MeetingTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesVisits");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "DiscussionTypes");

            migrationBuilder.DropTable(
                name: "MeetingTypes");
        }
    }
}
