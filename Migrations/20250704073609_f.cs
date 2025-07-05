using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class f : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZoneUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZoneUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.ZoneId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
