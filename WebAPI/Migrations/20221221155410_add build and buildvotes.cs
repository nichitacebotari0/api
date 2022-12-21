using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class addbuildandbuildvotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Build",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordUserId = table.Column<string>(type: "TEXT", nullable: false),
                    Augments = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Upvotes = table.Column<int>(type: "INTEGER", nullable: false),
                    Downvotes = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HeroId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Build", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Build_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordUserId = table.Column<string>(type: "TEXT", nullable: false),
                    VoteValue = table.Column<int>(type: "INTEGER", nullable: false),
                    BuildId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildVote_Build_BuildId",
                        column: x => x.BuildId,
                        principalTable: "Build",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Build_HeroId",
                table: "Build",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildVote_BuildId",
                table: "BuildVote",
                column: "BuildId");

            migrationBuilder.CreateIndex(
                name: "User_Build_Unique",
                table: "BuildVote",
                columns: new[] { "DiscordUserId", "BuildId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildVote");

            migrationBuilder.DropTable(
                name: "Build");
        }
    }
}
