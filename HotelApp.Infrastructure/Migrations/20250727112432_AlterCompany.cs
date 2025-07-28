using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BranchId",
                table: "Companies",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatedById",
                table: "Companies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_LastModifiedById",
                table: "Companies",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Branches_BranchId",
                table: "Companies",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_CreatedById",
                table: "Companies",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_LastModifiedById",
                table: "Companies",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Branches_BranchId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_CreatedById",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_LastModifiedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_BranchId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CreatedById",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_LastModifiedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Companies");
        }
    }
}
