using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignBranchToRoomTypeAndRoomStatusAndRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_CreatedById",
                table: "RoomStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_LastModifiedById",
                table: "RoomStatuses");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoomStatuses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoomStatuses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "RoomStatuses",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "BranchId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "BranchId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "BranchId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "BranchId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "BranchId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_BranchId",
                table: "RoomTypes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomStatuses_BranchId",
                table: "RoomStatuses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_BranchId",
                table: "Rates",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates",
                columns: new[] { "Code", "BranchId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Branches_BranchId",
                table: "Rates",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomStatuses_Branches_BranchId",
                table: "RoomStatuses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomStatuses_Users_CreatedById",
                table: "RoomStatuses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomStatuses_Users_LastModifiedById",
                table: "RoomStatuses",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_Branches_BranchId",
                table: "RoomTypes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Branches_BranchId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Branches_BranchId",
                table: "RoomStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_CreatedById",
                table: "RoomStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomStatuses_Users_LastModifiedById",
                table: "RoomStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_Branches_BranchId",
                table: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_BranchId",
                table: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_RoomStatuses_BranchId",
                table: "RoomStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Rates_BranchId",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Rates");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RoomStatuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoomStatuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

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
    }
}
