using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Users_CreatedById",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Users_LastModifiedById",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "OptionPrice",
                table: "Options");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Options",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DailyPrice",
                table: "Options",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayOnline",
                table: "Options",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraDailyPrice",
                table: "Options",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyPrice",
                table: "Options",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPrice",
                table: "Options",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeeklyPrice",
                table: "Options",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinChargeDayes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SkipHourly = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOnline = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    LastModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rate_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rate_Users_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomTypeRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    HourlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraDailyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeeklyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypeRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTypeRate_Rate_RateId",
                        column: x => x.RateId,
                        principalTable: "Rate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTypeRate_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rate_CreatedById",
                table: "Rate",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_LastModifiedById",
                table: "Rate",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeRate_RateId",
                table: "RoomTypeRate",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeRate_RoomTypeId",
                table: "RoomTypeRate",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Users_CreatedById",
                table: "Options",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Users_LastModifiedById",
                table: "Options",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Users_CreatedById",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Users_LastModifiedById",
                table: "Options");

            migrationBuilder.DropTable(
                name: "RoomTypeRate");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "DailyPrice",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "DisplayOnline",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "ExtraDailyPrice",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "HourlyPrice",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "WeeklyPrice",
                table: "Options");

            migrationBuilder.AddColumn<decimal>(
                name: "OptionPrice",
                table: "Options",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Users_CreatedById",
                table: "Options",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Users_LastModifiedById",
                table: "Options",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
