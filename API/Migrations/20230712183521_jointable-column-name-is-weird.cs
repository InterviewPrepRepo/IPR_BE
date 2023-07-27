using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPR_BE.Migrations
{
    /// <inheritdoc />
    public partial class jointablecolumnnameisweird : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Candidates_CandidateIdid",
                table: "CandidateSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Skills_SkillIdid",
                table: "CandidateSkill");

            migrationBuilder.RenameColumn(
                name: "SkillIdid",
                table: "CandidateSkill",
                newName: "Skillid");

            migrationBuilder.RenameColumn(
                name: "CandidateIdid",
                table: "CandidateSkill",
                newName: "Candidateid");

            migrationBuilder.RenameIndex(
                name: "IX_CandidateSkill_SkillIdid",
                table: "CandidateSkill",
                newName: "IX_CandidateSkill_Skillid");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Candidates_Candidateid",
                table: "CandidateSkill",
                column: "Candidateid",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Skills_Skillid",
                table: "CandidateSkill",
                column: "Skillid",
                principalTable: "Skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Candidates_Candidateid",
                table: "CandidateSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Skills_Skillid",
                table: "CandidateSkill");

            migrationBuilder.RenameColumn(
                name: "Skillid",
                table: "CandidateSkill",
                newName: "SkillIdid");

            migrationBuilder.RenameColumn(
                name: "Candidateid",
                table: "CandidateSkill",
                newName: "CandidateIdid");

            migrationBuilder.RenameIndex(
                name: "IX_CandidateSkill_Skillid",
                table: "CandidateSkill",
                newName: "IX_CandidateSkill_SkillIdid");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Candidates_CandidateIdid",
                table: "CandidateSkill",
                column: "CandidateIdid",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Skills_SkillIdid",
                table: "CandidateSkill",
                column: "SkillIdid",
                principalTable: "Skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
