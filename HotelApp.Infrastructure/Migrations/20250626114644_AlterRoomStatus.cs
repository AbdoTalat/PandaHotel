using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRoomStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "RoomTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "RoomStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RoomStatuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RoomStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "RoomStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "RoomStatuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Rooms",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Reservations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Rates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Options",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Guests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Floors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Branches",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedDate", "IsActive", "LastModifiedById", "LastModifiedDate" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedById", "CreatedDate", "IsActive", "LastModifiedById", "LastModifiedDate" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedById", "CreatedDate", "IsActive", "LastModifiedById", "LastModifiedDate" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedById", "CreatedDate", "IsActive", "LastModifiedById", "LastModifiedDate" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedById", "CreatedDate", "IsActive", "LastModifiedById", "LastModifiedDate" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_RoomStatuses_CreatedById",
                table: "RoomStatuses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoomStatuses_LastModifiedById",
                table: "RoomStatuses",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomStatuses_Users_CreatedById",
                table: "RoomStatuses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomStatuses_Users_LastModifiedById",
                table: "RoomStatuses",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_CreatedById",
                table: "RoomStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_LastModifiedById",
                table: "RoomStatuses");

            migrationBuilder.DropIndex(
                name: "IX_RoomStatuses_CreatedById",
                table: "RoomStatuses");

            migrationBuilder.DropIndex(
                name: "IX_RoomStatuses_LastModifiedById",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "RoomStatuses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "RoomTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Rooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Rates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Options",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Guests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Floors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Branches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
