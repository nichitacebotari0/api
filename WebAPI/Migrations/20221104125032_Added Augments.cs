using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class AddedAugments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbilityType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Active",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Active", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AugmentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AugmentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    HeroClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hero", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hero_HeroClass_HeroClassId",
                        column: x => x.HeroClassId,
                        principalTable: "HeroClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Augment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    AugmentCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    AbilityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Augment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Augment_AbilityType_AbilityTypeId",
                        column: x => x.AbilityTypeId,
                        principalTable: "AbilityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                        column: x => x.AugmentCategoryId,
                        principalTable: "AugmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Augment_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Hero_HeroClassId",
                table: "Hero",
                column: "HeroClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Active");

            migrationBuilder.DropTable(
                name: "Augment");

            migrationBuilder.DropTable(
                name: "AbilityType");

            migrationBuilder.DropTable(
                name: "AugmentCategory");

            migrationBuilder.DropTable(
                name: "Hero");

            migrationBuilder.DropTable(
                name: "HeroClass");
        }
    }
}
