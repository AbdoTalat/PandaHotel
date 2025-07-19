using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_AspNetUsers_ApplicationUserId",
                table: "UserBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_UserBranches_ApplicationUserId",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserBranches");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Reservations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ApplicationUserId",
                table: "Reservations",
                newName: "IX_Reservations_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "RoomTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "RoomTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RolePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Floors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Floors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_UserId",
                table: "UserBranches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_CreatedById",
                table: "RoomTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_LastModifiedById",
                table: "RoomTypes",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CreatedById",
                table: "Rooms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LastModifiedById",
                table: "Rooms",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CreatedById",
                table: "Reservations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LastModifiedById",
                table: "Reservations",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_CreatedById",
                table: "Guests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_LastModifiedById",
                table: "Guests",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_CreatedById",
                table: "Floors",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_LastModifiedById",
                table: "Floors",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedById",
                table: "Branches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_LastModifiedById",
                table: "Branches",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastModifiedById",
                table: "AspNetUsers",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_CreatedById",
                table: "AspNetRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_LastModifiedById",
                table: "AspNetRoles",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_CreatedById",
                table: "AspNetRoles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_LastModifiedById",
                table: "AspNetRoles",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LastModifiedById",
                table: "AspNetUsers",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                table: "Branches",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_LastModifiedById",
                table: "Branches",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_AspNetUsers_CreatedById",
                table: "Floors",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_AspNetUsers_LastModifiedById",
                table: "Floors",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_AspNetUsers_CreatedById",
                table: "Guests",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_AspNetUsers_LastModifiedById",
                table: "Guests",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_CreatedById",
                table: "Reservations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_LastModifiedById",
                table: "Reservations",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_LastModifiedById",
                table: "Rooms",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_AspNetUsers_CreatedById",
                table: "RoomTypes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_AspNetUsers_LastModifiedById",
                table: "RoomTypes",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_AspNetUsers_UserId",
                table: "UserBranches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                table: "UserBranches",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_LastModifiedById",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LastModifiedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_LastModifiedById",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_AspNetUsers_CreatedById",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Floors_AspNetUsers_LastModifiedById",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Guests_AspNetUsers_CreatedById",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Guests_AspNetUsers_LastModifiedById",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_CreatedById",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_LastModifiedById",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_CreatedById",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_LastModifiedById",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_AspNetUsers_CreatedById",
                table: "RoomTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_AspNetUsers_LastModifiedById",
                table: "RoomTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_AspNetUsers_UserId",
                table: "UserBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_UserBranches_UserId",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_CreatedById",
                table: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_LastModifiedById",
                table: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_CreatedById",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LastModifiedById",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CreatedById",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_LastModifiedById",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Guests_CreatedById",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_LastModifiedById",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Floors_CreatedById",
                table: "Floors");

            migrationBuilder.DropIndex(
                name: "IX_Floors_LastModifiedById",
                table: "Floors");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CreatedById",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_LastModifiedById",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LastModifiedById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_LastModifiedById",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Floors");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Floors");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservations",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                newName: "IX_Reservations_ApplicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "UserBranches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_ApplicationUserId",
                table: "UserBranches",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_AspNetUsers_ApplicationUserId",
                table: "UserBranches",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                table: "UserBranches",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
