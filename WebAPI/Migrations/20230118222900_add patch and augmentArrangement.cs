using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class addpatchandaugmentArrangement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    GameDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WebsiteTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AugmentArrangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AugmentArrangement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AugmentArrangement_Patch_PatchId",
                        column: x => x.PatchId,
                        principalTable: "Patch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AugmentSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AugmentCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    AugmentArrangementId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AugmentSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AugmentSlot_AugmentArrangement_AugmentArrangementId",
                        column: x => x.AugmentArrangementId,
                        principalTable: "AugmentArrangement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AugmentSlot_AugmentCategory_AugmentCategoryId",
                        column: x => x.AugmentCategoryId,
                        principalTable: "AugmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Patch",
                columns: new[] { "Id", "GameDate", "Title", "Version", "WebsiteTimeUtc" },
                values: new object[] { 1, new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Initial Patch", "initial", new DateTime(2022, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_AugmentArrangement_PatchId",
                table: "AugmentArrangement",
                column: "PatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AugmentSlot_AugmentArrangementId",
                table: "AugmentSlot",
                column: "AugmentArrangementId");

            migrationBuilder.CreateIndex(
                name: "IX_AugmentSlot_AugmentCategoryId",
                table: "AugmentSlot",
                column: "AugmentCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AugmentSlot");

            migrationBuilder.DropTable(
                name: "AugmentArrangement");

            migrationBuilder.DropTable(
                name: "Patch");
        }
    }
}
