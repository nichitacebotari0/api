using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedAugmentEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO AugmentEvent (AugmentId,Name,Description,ImagePath,AugmentCategoryId,AbilityTypeId,HeroId,""Action"", PatchId)
SELECT Id,Name,Description,ImagePath,AugmentCategoryId,AbilityTypeId,HeroId, 0, (SELECT Id FROM Patch ORDER BY Id LIMIT 1) AS  PatchId FROM Augment ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
