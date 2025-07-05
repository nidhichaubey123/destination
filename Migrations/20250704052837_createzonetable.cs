using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class createzonetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OperationCreatedBy",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_DiscussionTypeId",
                table: "SalesVisits",
                column: "DiscussionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_MeetingTypeId",
                table: "SalesVisits",
                column: "MeetingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_UserId",
                table: "SalesVisits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesVisits_DiscussionTypes_DiscussionTypeId",
                table: "SalesVisits",
                column: "DiscussionTypeId",
                principalTable: "DiscussionTypes",
                principalColumn: "DiscussionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesVisits_MeetingTypes_MeetingTypeId",
                table: "SalesVisits",
                column: "MeetingTypeId",
                principalTable: "MeetingTypes",
                principalColumn: "MeetingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesVisits_Users_UserId",
                table: "SalesVisits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesVisits_DiscussionTypes_DiscussionTypeId",
                table: "SalesVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesVisits_MeetingTypes_MeetingTypeId",
                table: "SalesVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesVisits_Users_UserId",
                table: "SalesVisits");

            migrationBuilder.DropIndex(
                name: "IX_SalesVisits_DiscussionTypeId",
                table: "SalesVisits");

            migrationBuilder.DropIndex(
                name: "IX_SalesVisits_MeetingTypeId",
                table: "SalesVisits");

            migrationBuilder.DropIndex(
                name: "IX_SalesVisits_UserId",
                table: "SalesVisits");

            migrationBuilder.DropColumn(
                name: "RoleCreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "OperationCreatedBy",
                table: "Operations");
        }
    }
}
