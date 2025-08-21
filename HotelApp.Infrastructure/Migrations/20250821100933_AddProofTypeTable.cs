using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProofTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfProof",
                table: "Guests");

            migrationBuilder.AddColumn<int>(
                name: "ProofTypeId",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProofType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    LastModifiedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofType_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofType_Users_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_ProofTypeId",
                table: "Guests",
                column: "ProofTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofType_CreatedById",
                table: "ProofType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProofType_LastModifiedById",
                table: "ProofType",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_ProofType_ProofTypeId",
                table: "Guests",
                column: "ProofTypeId",
                principalTable: "ProofType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_ProofType_ProofTypeId",
                table: "Guests");

            migrationBuilder.DropTable(
                name: "ProofType");

            migrationBuilder.DropIndex(
                name: "IX_Guests_ProofTypeId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "ProofTypeId",
                table: "Guests");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfProof",
                table: "Guests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
