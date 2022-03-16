using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Database.Migrations
{
    public partial class fixedSubscribe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Films_FilmId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_FilmId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Genres");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscribeId",
                table: "Films",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FilmGenre",
                columns: table => new
                {
                    FilmsId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmGenre", x => new { x.FilmsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_FilmGenre_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_SubscribeId",
                table: "Films",
                column: "SubscribeId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenre_GenresId",
                table: "FilmGenre",
                column: "GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Subscribes_SubscribeId",
                table: "Films",
                column: "SubscribeId",
                principalTable: "Subscribes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Subscribes_SubscribeId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "FilmGenre");

            migrationBuilder.DropIndex(
                name: "IX_Films_SubscribeId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "SubscribeId",
                table: "Films");

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId",
                table: "Genres",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_FilmId",
                table: "Genres",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Films_FilmId",
                table: "Genres",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");
        }
    }
}
