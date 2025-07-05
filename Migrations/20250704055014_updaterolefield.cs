using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMCPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class updaterolefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleCreatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "RoleUpdatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RoleUpdatedOn",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OperationCreatedBy",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OperationUpdatedBy",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationUpdatedOn",
                table: "Operations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleUpdatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleUpdatedOn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "OperationUpdatedBy",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OperationUpdatedOn",
                table: "Operations");

            migrationBuilder.AlterColumn<string>(
                name: "RoleCreatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OperationCreatedBy",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
