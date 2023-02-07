using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class removeAugmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Augment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Augment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AbilityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AugmentCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Augment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Augment_AbilityType_AbilityTypeId",
                        column: x => x.AbilityTypeId,
                        principalTable: "AbilityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                        column: x => x.AugmentCategoryId,
                        principalTable: "AugmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Augment_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Augment_AbilityTypeId",
                table: "Augment",
                column: "AbilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Augment_AugmentCategoryId",
                table: "Augment",
                column: "AugmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Augment_HeroId",
                table: "Augment",
                column: "HeroId");
        }
    }
}
