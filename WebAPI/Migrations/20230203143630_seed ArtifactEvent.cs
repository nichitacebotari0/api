using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedArtifactEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO ArtifactEvent (ArtifactId,Name,Description,ImagePath,ArtifactTypeId,""Action"",PatchId)
SELECT Id,Name,Description,ImagePath,ArtifactTypeId, 0, (SELECT Id FROM Patch ORDER BY Id LIMIT 1) AS PatchId FROM Artifact");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
