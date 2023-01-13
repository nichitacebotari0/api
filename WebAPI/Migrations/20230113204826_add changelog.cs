using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class addchangelog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BuildVote_BuildId",
                table: "BuildVote");

            migrationBuilder.DropIndex(
                name: "User_Build_Unique",
                table: "BuildVote");

            migrationBuilder.CreateTable(
                name: "ChangeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    DiscordId = table.Column<string>(type: "TEXT", nullable: false),
                    DiscordNick = table.Column<string>(type: "TEXT", nullable: false),
                    SummaryPrevious = table.Column<string>(type: "TEXT", nullable: true),
                    SummaryNext = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "User_Build_Unique",
                table: "BuildVote",
                columns: new[] { "BuildId", "DiscordUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLog_CreatedAtUtc",
                table: "ChangeLog",
                column: "CreatedAtUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeLog");

            migrationBuilder.DropIndex(
                name: "User_Build_Unique",
                table: "BuildVote");

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
    }
}
