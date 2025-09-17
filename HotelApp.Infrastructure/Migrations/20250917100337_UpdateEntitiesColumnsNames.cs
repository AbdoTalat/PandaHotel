using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesColumnsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFullAdmin",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsBasic",
                table: "Roles",
                newName: "IsSystem");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Reservations",
                newName: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsSystem",
                table: "Roles",
                newName: "IsBasic");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Reservations",
                newName: "Comment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isFullAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
