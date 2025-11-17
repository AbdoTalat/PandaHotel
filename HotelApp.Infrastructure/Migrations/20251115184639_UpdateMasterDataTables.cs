using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMasterDataTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_DropDownItems_TransactionTypeId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "DropDownItems");

            migrationBuilder.DropTable(
                name: "DropDownTypes");

            migrationBuilder.CreateTable(
                name: "MasterDataTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterDataItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DropDownTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterDataItems_MasterDataTypes_DropDownTypeId",
                        column: x => x.DropDownTypeId,
                        principalTable: "MasterDataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MasterDataTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "TransactionType" });

            migrationBuilder.InsertData(
                table: "MasterDataItems",
                columns: new[] { "Id", "DropDownTypeId", "IsActive", "Name", "Value" },
                values: new object[,]
                {
                    { 1, 1, true, "Payment", (short)1 },
                    { 2, 1, true, "Refund", (short)2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDataItems_DropDownTypeId",
                table: "MasterDataItems",
                column: "DropDownTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_MasterDataItems_TransactionTypeId",
                table: "Payments",
                column: "TransactionTypeId",
                principalTable: "MasterDataItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_MasterDataItems_TransactionTypeId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "MasterDataItems");

            migrationBuilder.DropTable(
                name: "MasterDataTypes");

            migrationBuilder.CreateTable(
                name: "DropDownTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DropDownItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DropDownTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropDownItems_DropDownTypes_DropDownTypeId",
                        column: x => x.DropDownTypeId,
                        principalTable: "DropDownTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DropDownTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "TransactionType" });

            migrationBuilder.InsertData(
                table: "DropDownItems",
                columns: new[] { "Id", "DropDownTypeId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, true, "Payment" },
                    { 2, 1, true, "Refund" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropDownItems_DropDownTypeId",
                table: "DropDownItems",
                column: "DropDownTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_DropDownItems_TransactionTypeId",
                table: "Payments",
                column: "TransactionTypeId",
                principalTable: "DropDownItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
