using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class addcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesVisits_DiscussionTypes_DiscussionTypeId",
                table: "SalesVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesVisits_MeetingTypes_MeetingTypeId",
                table: "SalesVisits");

            migrationBuilder.DropIndex(
                name: "IX_SalesVisits_DiscussionTypeId",
                table: "SalesVisits");

            migrationBuilder.DropIndex(
                name: "IX_SalesVisits_MeetingTypeId",
                table: "SalesVisits");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "SalesVisits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "SalesVisits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "SalesVisits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "SalesVisits",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscussionTypeName",
                table: "DiscussionTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SalesVisits");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SalesVisits");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "SalesVisits");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SalesVisits");

            migrationBuilder.AlterColumn<string>(
                name: "DiscussionTypeName",
                table: "DiscussionTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_DiscussionTypeId",
                table: "SalesVisits",
                column: "DiscussionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesVisits_MeetingTypeId",
                table: "SalesVisits",
                column: "MeetingTypeId");

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
        }
    }
}
