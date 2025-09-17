using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCalculationTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalculationTypeId",
                table: "SystemSettings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalculationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_CalculationTypeId",
                table: "SystemSettings",
                column: "CalculationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_CalculationTypes_CalculationTypeId",
                table: "SystemSettings",
                column: "CalculationTypeId",
                principalTable: "CalculationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_CalculationTypes_CalculationTypeId",
                table: "SystemSettings");

            migrationBuilder.DropTable(
                name: "CalculationTypes");

            migrationBuilder.DropIndex(
                name: "IX_SystemSettings_CalculationTypeId",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "CalculationTypeId",
                table: "SystemSettings");
        }
    }
}
