using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addAugmentEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AugmentEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AugmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    AugmentCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    AbilityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<int>(type: "INTEGER", nullable: false),
                    PatchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AugmentEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AugmentEvent_AbilityType_AbilityTypeId",
                        column: x => x.AbilityTypeId,
                        principalTable: "AbilityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AugmentEvent_AugmentCategory_AugmentCategoryId",
                        column: x => x.AugmentCategoryId,
                        principalTable: "AugmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AugmentEvent_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AugmentEvent_Patch_PatchId",
                        column: x => x.PatchId,
                        principalTable: "Patch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AugmentEvent_AbilityTypeId",
                table: "AugmentEvent",
                column: "AbilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AugmentEvent_AugmentCategoryId",
                table: "AugmentEvent",
                column: "AugmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AugmentEvent_HeroId",
                table: "AugmentEvent",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_AugmentEvent_PatchId",
                table: "AugmentEvent",
                column: "PatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AugmentEvent");
        }
    }
}
