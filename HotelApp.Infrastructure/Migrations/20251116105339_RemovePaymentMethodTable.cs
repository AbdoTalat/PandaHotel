using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaymentMethodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentMethods_PaymentMethodId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.InsertData(
                table: "MasterDataTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "PaymentMethod" });

            migrationBuilder.InsertData(
                table: "MasterDataItems",
                columns: new[] { "Id", "IsActive", "MasterDataTypeId", "Name", "Value" },
                values: new object[,]
                {
                    { 3, true, 2, "Cash", (short)1 },
                    { 4, true, 2, "CreditCard", (short)2 },
                    { 5, true, 2, "DebitCard", (short)3 },
                    { 6, true, 2, "BankTransfer", (short)4 },
                    { 7, true, 2, "Wallet", (short)5 },
                    { 8, true, 2, "Cheque", (short)6 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_MasterDataItems_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId",
                principalTable: "MasterDataItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_MasterDataItems_PaymentMethodId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MasterDataTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentMethods_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
