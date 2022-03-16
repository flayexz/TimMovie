using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Database.Migrations
{
    public partial class fixedSubscribes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Subscribes_SubscribeId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_SubscribeId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "SubscribeId",
                table: "Films");

            migrationBuilder.CreateTable(
                name: "FilmSubscribe",
                columns: table => new
                {
                    FilmsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmSubscribe", x => new { x.FilmsId, x.SubscribesId });
                    table.ForeignKey(
                        name: "FK_FilmSubscribe_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmSubscribe_Subscribes_SubscribesId",
                        column: x => x.SubscribesId,
                        principalTable: "Subscribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmSubscribe_SubscribesId",
                table: "FilmSubscribe",
                column: "SubscribesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmSubscribe");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscribeId",
                table: "Films",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_SubscribeId",
                table: "Films",
                column: "SubscribeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Subscribes_SubscribeId",
                table: "Films",
                column: "SubscribeId",
                principalTable: "Subscribes",
                principalColumn: "Id");
        }
    }
}
