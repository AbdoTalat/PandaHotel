using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterSystemSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGuestAddressRequired",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestDateOfBirthRequired",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestPhoneRequired",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestProofNumberRequired",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestProofTypeRequired",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGuestAddressRequired",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "IsGuestDateOfBirthRequired",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "IsGuestPhoneRequired",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "IsGuestProofNumberRequired",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "IsGuestProofTypeRequired",
                table: "SystemSettings");
        }
    }
}
