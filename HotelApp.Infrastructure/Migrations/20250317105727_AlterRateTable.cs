using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Users_CreatedById",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Users_LastModifiedById",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeRate_Rate_RateId",
                table: "RoomTypeRate");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeRate_RoomTypes_RoomTypeId",
                table: "RoomTypeRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTypeRate",
                table: "RoomTypeRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "Rate");

            migrationBuilder.RenameTable(
                name: "RoomTypeRate",
                newName: "RoomTypeRates");

            migrationBuilder.RenameTable(
                name: "Rate",
                newName: "Rates");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTypeRate_RoomTypeId",
                table: "RoomTypeRates",
                newName: "IX_RoomTypeRates_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTypeRate_RateId",
                table: "RoomTypeRates",
                newName: "IX_RoomTypeRates_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_LastModifiedById",
                table: "Rates",
                newName: "IX_Rates_LastModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_CreatedById",
                table: "Rates",
                newName: "IX_Rates_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Rates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTypeRates",
                table: "RoomTypeRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Users_CreatedById",
                table: "Rates",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Users_LastModifiedById",
                table: "Rates",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeRates_Rates_RateId",
                table: "RoomTypeRates",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeRates_RoomTypes_RoomTypeId",
                table: "RoomTypeRates",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Users_CreatedById",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Users_LastModifiedById",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeRates_Rates_RateId",
                table: "RoomTypeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypeRates_RoomTypes_RoomTypeId",
                table: "RoomTypeRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTypeRates",
                table: "RoomTypeRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "RoomTypeRates",
                newName: "RoomTypeRate");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTypeRates_RoomTypeId",
                table: "RoomTypeRate",
                newName: "IX_RoomTypeRate_RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTypeRates_RateId",
                table: "RoomTypeRate",
                newName: "IX_RoomTypeRate_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_LastModifiedById",
                table: "Rate",
                newName: "IX_Rate_LastModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_CreatedById",
                table: "Rate",
                newName: "IX_Rate_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Rate",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTypeRate",
                table: "RoomTypeRate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "Rate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Users_CreatedById",
                table: "Rate",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Users_LastModifiedById",
                table: "Rate",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeRate_Rate_RateId",
                table: "RoomTypeRate",
                column: "RateId",
                principalTable: "Rate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypeRate_RoomTypes_RoomTypeId",
                table: "RoomTypeRate",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
