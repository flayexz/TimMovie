using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Database.Migrations
{
    public partial class fixedFilms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_UserId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_UserId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Films");

            migrationBuilder.CreateTable(
                name: "FilmUser",
                columns: table => new
                {
                    FilmsWatchLaterId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersWatchLaterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmUser", x => new { x.FilmsWatchLaterId, x.UsersWatchLaterId });
                    table.ForeignKey(
                        name: "FK_FilmUser_AspNetUsers_UsersWatchLaterId",
                        column: x => x.UsersWatchLaterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmUser_Films_FilmsWatchLaterId",
                        column: x => x.FilmsWatchLaterId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmUser_UsersWatchLaterId",
                table: "FilmUser",
                column: "UsersWatchLaterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Films",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_UserId",
                table: "Films",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_UserId",
                table: "Films",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
