using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMasterDataType02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDataItems_MasterDataTypes_DropDownTypeId",
                table: "MasterDataItems");

            migrationBuilder.RenameColumn(
                name: "DropDownTypeId",
                table: "MasterDataItems",
                newName: "MasterDataTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDataItems_DropDownTypeId",
                table: "MasterDataItems",
                newName: "IX_MasterDataItems_MasterDataTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDataItems_MasterDataTypes_MasterDataTypeId",
                table: "MasterDataItems",
                column: "MasterDataTypeId",
                principalTable: "MasterDataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDataItems_MasterDataTypes_MasterDataTypeId",
                table: "MasterDataItems");

            migrationBuilder.RenameColumn(
                name: "MasterDataTypeId",
                table: "MasterDataItems",
                newName: "DropDownTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDataItems_MasterDataTypeId",
                table: "MasterDataItems",
                newName: "IX_MasterDataItems_DropDownTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDataItems_MasterDataTypes_DropDownTypeId",
                table: "MasterDataItems",
                column: "DropDownTypeId",
                principalTable: "MasterDataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
