using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Database.Migrations
{
    public partial class fixedActors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_Films_FilmId",
                table: "Actors");

            migrationBuilder.DropForeignKey(
                name: "FK_Producers_Films_FilmId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Producers_FilmId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Actors_FilmId",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Actors");

            migrationBuilder.CreateTable(
                name: "ActorFilm",
                columns: table => new
                {
                    ActorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilmsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorFilm", x => new { x.ActorsId, x.FilmsId });
                    table.ForeignKey(
                        name: "FK_ActorFilm_Actors_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorFilm_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmProducer",
                columns: table => new
                {
                    FilmsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProducersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmProducer", x => new { x.FilmsId, x.ProducersId });
                    table.ForeignKey(
                        name: "FK_FilmProducer_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmProducer_Producers_ProducersId",
                        column: x => x.ProducersId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorFilm_FilmsId",
                table: "ActorFilm",
                column: "FilmsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmProducer_ProducersId",
                table: "FilmProducer",
                column: "ProducersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorFilm");

            migrationBuilder.DropTable(
                name: "FilmProducer");

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId",
                table: "Producers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId",
                table: "Actors",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producers_FilmId",
                table: "Producers",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_FilmId",
                table: "Actors",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_Films_FilmId",
                table: "Actors",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producers_Films_FilmId",
                table: "Producers",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");
        }
    }
}
