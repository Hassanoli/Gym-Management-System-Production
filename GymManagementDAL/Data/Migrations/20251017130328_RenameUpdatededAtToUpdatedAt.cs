using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameUpdatededAtToUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "Trainers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "Sessions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "Plan",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "MemberShips",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "MemberSessions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "Members",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatededAt",
                table: "Categories",
                newName: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Trainers",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Sessions",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Plan",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "MemberShips",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "MemberSessions",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Members",
                newName: "UpdatededAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "UpdatededAt");
        }
    }
}
