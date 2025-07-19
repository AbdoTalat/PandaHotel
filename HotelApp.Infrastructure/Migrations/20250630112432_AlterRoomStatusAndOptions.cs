using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRoomStatusAndOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "RoomStatuses",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: "#20BF7E");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: "#20BF7E");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: "#20BF7E");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: "#20BF7E");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: "#20BF7E");

            migrationBuilder.CreateIndex(
                name: "IX_Options_BranchId",
                table: "Options",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Branches_BranchId",
                table: "Options",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Branches_BranchId",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_BranchId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Options");
        }
    }
}
