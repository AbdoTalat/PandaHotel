using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProofType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_ProofType_ProofTypeId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofType_Users_CreatedById",
                table: "ProofType");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofType_Users_LastModifiedById",
                table: "ProofType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProofType",
                table: "ProofType");

            migrationBuilder.RenameTable(
                name: "ProofType",
                newName: "ProofTypes");

            migrationBuilder.RenameIndex(
                name: "IX_ProofType_LastModifiedById",
                table: "ProofTypes",
                newName: "IX_ProofTypes_LastModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProofType_CreatedById",
                table: "ProofTypes",
                newName: "IX_ProofTypes_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProofTypes",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProofTypes",
                table: "ProofTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_ProofTypes_ProofTypeId",
                table: "Guests",
                column: "ProofTypeId",
                principalTable: "ProofTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofTypes_Users_CreatedById",
                table: "ProofTypes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofTypes_Users_LastModifiedById",
                table: "ProofTypes",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_ProofTypes_ProofTypeId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofTypes_Users_CreatedById",
                table: "ProofTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofTypes_Users_LastModifiedById",
                table: "ProofTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProofTypes",
                table: "ProofTypes");

            migrationBuilder.RenameTable(
                name: "ProofTypes",
                newName: "ProofType");

            migrationBuilder.RenameIndex(
                name: "IX_ProofTypes_LastModifiedById",
                table: "ProofType",
                newName: "IX_ProofType_LastModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProofTypes_CreatedById",
                table: "ProofType",
                newName: "IX_ProofType_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProofType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProofType",
                table: "ProofType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_ProofType_ProofTypeId",
                table: "Guests",
                column: "ProofTypeId",
                principalTable: "ProofType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofType_Users_CreatedById",
                table: "ProofType",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProofType_Users_LastModifiedById",
                table: "ProofType",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
