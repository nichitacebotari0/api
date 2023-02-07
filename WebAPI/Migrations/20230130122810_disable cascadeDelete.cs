using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class disablecascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artifact_ArtifactType_ArtifactTypeId",
                table: "Artifact");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_AbilityType_AbilityTypeId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_Hero_HeroId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentArrangement_Patch_PatchId",
                table: "AugmentArrangement");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_AbilityType_AbilityTypeId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_AugmentCategory_AugmentCategoryId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_Hero_HeroId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_Patch_PatchId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentSlot_AugmentArrangement_AugmentArrangementId",
                table: "AugmentSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentSlot_AugmentCategory_AugmentCategoryId",
                table: "AugmentSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_Build_Hero_HeroId",
                table: "Build");

            migrationBuilder.DropForeignKey(
                name: "FK_BuildVote_Build_BuildId",
                table: "BuildVote");

            migrationBuilder.DropForeignKey(
                name: "FK_Hero_HeroClass_HeroClassId",
                table: "Hero");

            migrationBuilder.AddForeignKey(
                name: "FK_Artifact_ArtifactType_ArtifactTypeId",
                table: "Artifact",
                column: "ArtifactTypeId",
                principalTable: "ArtifactType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_AbilityType_AbilityTypeId",
                table: "Augment",
                column: "AbilityTypeId",
                principalTable: "AbilityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                table: "Augment",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_Hero_HeroId",
                table: "Augment",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentArrangement_Patch_PatchId",
                table: "AugmentArrangement",
                column: "PatchId",
                principalTable: "Patch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_AbilityType_AbilityTypeId",
                table: "AugmentEvent",
                column: "AbilityTypeId",
                principalTable: "AbilityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_AugmentCategory_AugmentCategoryId",
                table: "AugmentEvent",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_Hero_HeroId",
                table: "AugmentEvent",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_Patch_PatchId",
                table: "AugmentEvent",
                column: "PatchId",
                principalTable: "Patch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentSlot_AugmentArrangement_AugmentArrangementId",
                table: "AugmentSlot",
                column: "AugmentArrangementId",
                principalTable: "AugmentArrangement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentSlot_AugmentCategory_AugmentCategoryId",
                table: "AugmentSlot",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Build_Hero_HeroId",
                table: "Build",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildVote_Build_BuildId",
                table: "BuildVote",
                column: "BuildId",
                principalTable: "Build",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_HeroClass_HeroClassId",
                table: "Hero",
                column: "HeroClassId",
                principalTable: "HeroClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artifact_ArtifactType_ArtifactTypeId",
                table: "Artifact");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_AbilityType_AbilityTypeId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_Augment_Hero_HeroId",
                table: "Augment");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentArrangement_Patch_PatchId",
                table: "AugmentArrangement");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_AbilityType_AbilityTypeId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_AugmentCategory_AugmentCategoryId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_Hero_HeroId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentEvent_Patch_PatchId",
                table: "AugmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentSlot_AugmentArrangement_AugmentArrangementId",
                table: "AugmentSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_AugmentSlot_AugmentCategory_AugmentCategoryId",
                table: "AugmentSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_Build_Hero_HeroId",
                table: "Build");

            migrationBuilder.DropForeignKey(
                name: "FK_BuildVote_Build_BuildId",
                table: "BuildVote");

            migrationBuilder.DropForeignKey(
                name: "FK_Hero_HeroClass_HeroClassId",
                table: "Hero");

            migrationBuilder.AddForeignKey(
                name: "FK_Artifact_ArtifactType_ArtifactTypeId",
                table: "Artifact",
                column: "ArtifactTypeId",
                principalTable: "ArtifactType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_AbilityType_AbilityTypeId",
                table: "Augment",
                column: "AbilityTypeId",
                principalTable: "AbilityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_AugmentCategory_AugmentCategoryId",
                table: "Augment",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Augment_Hero_HeroId",
                table: "Augment",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentArrangement_Patch_PatchId",
                table: "AugmentArrangement",
                column: "PatchId",
                principalTable: "Patch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_AbilityType_AbilityTypeId",
                table: "AugmentEvent",
                column: "AbilityTypeId",
                principalTable: "AbilityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_AugmentCategory_AugmentCategoryId",
                table: "AugmentEvent",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_Hero_HeroId",
                table: "AugmentEvent",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentEvent_Patch_PatchId",
                table: "AugmentEvent",
                column: "PatchId",
                principalTable: "Patch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentSlot_AugmentArrangement_AugmentArrangementId",
                table: "AugmentSlot",
                column: "AugmentArrangementId",
                principalTable: "AugmentArrangement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AugmentSlot_AugmentCategory_AugmentCategoryId",
                table: "AugmentSlot",
                column: "AugmentCategoryId",
                principalTable: "AugmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Build_Hero_HeroId",
                table: "Build",
                column: "HeroId",
                principalTable: "Hero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildVote_Build_BuildId",
                table: "BuildVote",
                column: "BuildId",
                principalTable: "Build",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_HeroClass_HeroClassId",
                table: "Hero",
                column: "HeroClassId",
                principalTable: "HeroClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
