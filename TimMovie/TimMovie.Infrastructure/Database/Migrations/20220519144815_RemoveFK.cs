using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Infrastructure.Database.Migrations
{
    public partial class RemoveFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStatus_AspNetUsers_UserGuid",
                table: "UserStatus");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "UserStatus",
                newName: "UserForeignKey");

            migrationBuilder.RenameIndex(
                name: "IX_UserStatus_UserGuid",
                table: "UserStatus",
                newName: "IX_UserStatus_UserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStatus_AspNetUsers_UserForeignKey",
                table: "UserStatus",
                column: "UserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStatus_AspNetUsers_UserForeignKey",
                table: "UserStatus");

            migrationBuilder.RenameColumn(
                name: "UserForeignKey",
                table: "UserStatus",
                newName: "UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_UserStatus_UserForeignKey",
                table: "UserStatus",
                newName: "IX_UserStatus_UserGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStatus_AspNetUsers_UserGuid",
                table: "UserStatus",
                column: "UserGuid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
