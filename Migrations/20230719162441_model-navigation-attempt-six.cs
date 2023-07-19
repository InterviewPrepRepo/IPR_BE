using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPR_BE.Migrations
{
    /// <inheritdoc />
    public partial class modelnavigationattemptsix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Candidates_Candidateid",
                table: "CandidateSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateSkill_Skills_Skillid",
                table: "CandidateSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttempts_Candidates_candidateId",
                table: "TestAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestTags",
                table: "TestTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSectionQuestions",
                table: "TestSectionQuestions");

            migrationBuilder.DropColumn(
                name: "testQuestionId",
                table: "TestAttemptQuestionSections");

            migrationBuilder.RenameTable(
                name: "TestTags",
                newName: "TestTag");

            migrationBuilder.RenameTable(
                name: "TestSectionQuestions",
                newName: "TestSectionQuestion");

            migrationBuilder.RenameColumn(
                name: "testAttemptId",
                table: "TestAttemptSections",
                newName: "testAttemptattemptId");

            migrationBuilder.RenameColumn(
                name: "candidateId",
                table: "TestAttempts",
                newName: "candidateid");

            migrationBuilder.RenameIndex(
                name: "IX_TestAttempts_candidateId",
                table: "TestAttempts",
                newName: "IX_TestAttempts_candidateid");

            migrationBuilder.AddColumn<int>(
                name: "testSectionQuestionid",
                table: "TestAttemptQuestionSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestTag",
                table: "TestTag",
                column: "testTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSectionQuestion",
                table: "TestSectionQuestion",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_TestSections_testId",
                table: "TestSections",
                column: "testId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttemptSections_testAttemptattemptId",
                table: "TestAttemptSections",
                column: "testAttemptattemptId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttemptSections_testSectionId",
                table: "TestAttemptSections",
                column: "testSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_testId",
                table: "TestAttempts",
                column: "testId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttemptQuestionSections_testAttemptSectionId",
                table: "TestAttemptQuestionSections",
                column: "testAttemptSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttemptQuestionSections_testSectionQuestionid",
                table: "TestAttemptQuestionSections",
                column: "testSectionQuestionid");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_questionTypeId",
                table: "Questions",
                column: "questionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TestTag_tagId",
                table: "TestTag",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_TestTag_testId",
                table: "TestTag",
                column: "testId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSectionQuestion_questionId",
                table: "TestSectionQuestion",
                column: "questionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSectionQuestion_testSectionId",
                table: "TestSectionQuestion",
                column: "testSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Candidates_Candidateid",
                table: "CandidateSkill",
                column: "Candidateid",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateSkill_Skills_Skillid",
                table: "CandidateSkill",
                column: "Skillid",
                principalTable: "Skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_questionTypeId",
                table: "Questions",
                column: "questionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "questionTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttemptQuestionSections_TestAttemptSections_testAttemptSectionId",
                table: "TestAttemptQuestionSections",
                column: "testAttemptSectionId",
                principalTable: "TestAttemptSections",
                principalColumn: "testAttemptSectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttemptQuestionSections_TestSectionQuestion_testSectionQuestionid",
                table: "TestAttemptQuestionSections",
                column: "testSectionQuestionid",
                principalTable: "TestSectionQuestion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttempts_Candidates_candidateid",
                table: "TestAttempts",
                column: "candidateid",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttempts_Tests_testId",
                table: "TestAttempts",
                column: "testId",
                principalTable: "Tests",
                principalColumn: "testId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttemptSections_TestAttempts_testAttemptattemptId",
                table: "TestAttemptSections",
                column: "testAttemptattemptId",
                principalTable: "TestAttempts",
                principalColumn: "attemptId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttemptSections_TestSections_testSectionId",
                table: "TestAttemptSections",
                column: "testSectionId",
                principalTable: "TestSections",
                principalColumn: "testSectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSectionQuestion_Questions_questionId",
                table: "TestSectionQuestion",
                column: "questionId",
                principalTable: "Questions",
                principalColumn: "questionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSectionQuestion_TestSections_testSectionId",
                table: "TestSectionQuestion",
                column: "testSectionId",
                principalTable: "TestSections",
                principalColumn: "testSectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSections_Tests_testId",
                table: "TestSections",
                column: "testId",
                principalTable: "Tests",
                principalColumn: "testId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestTag_Tags_tagId",
                table: "TestTag",
                column: "tagId",
                principalTable: "Tags",
                principalColumn: "tagId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestTag_Tests_testId",
                table: "TestTag",
                column: "testId",
                principalTable: "Tests",
                principalColumn: "testId",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_questionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttemptQuestionSections_TestAttemptSections_testAttemptSectionId",
                table: "TestAttemptQuestionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttemptQuestionSections_TestSectionQuestion_testSectionQuestionid",
                table: "TestAttemptQuestionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttempts_Candidates_candidateid",
                table: "TestAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttempts_Tests_testId",
                table: "TestAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttemptSections_TestAttempts_testAttemptattemptId",
                table: "TestAttemptSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAttemptSections_TestSections_testSectionId",
                table: "TestAttemptSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSectionQuestion_Questions_questionId",
                table: "TestSectionQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSectionQuestion_TestSections_testSectionId",
                table: "TestSectionQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSections_Tests_testId",
                table: "TestSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TestTag_Tags_tagId",
                table: "TestTag");

            migrationBuilder.DropForeignKey(
                name: "FK_TestTag_Tests_testId",
                table: "TestTag");

            migrationBuilder.DropIndex(
                name: "IX_TestSections_testId",
                table: "TestSections");

            migrationBuilder.DropIndex(
                name: "IX_TestAttemptSections_testAttemptattemptId",
                table: "TestAttemptSections");

            migrationBuilder.DropIndex(
                name: "IX_TestAttemptSections_testSectionId",
                table: "TestAttemptSections");

            migrationBuilder.DropIndex(
                name: "IX_TestAttempts_testId",
                table: "TestAttempts");

            migrationBuilder.DropIndex(
                name: "IX_TestAttemptQuestionSections_testAttemptSectionId",
                table: "TestAttemptQuestionSections");

            migrationBuilder.DropIndex(
                name: "IX_TestAttemptQuestionSections_testSectionQuestionid",
                table: "TestAttemptQuestionSections");

            migrationBuilder.DropIndex(
                name: "IX_Questions_questionTypeId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestTag",
                table: "TestTag");

            migrationBuilder.DropIndex(
                name: "IX_TestTag_tagId",
                table: "TestTag");

            migrationBuilder.DropIndex(
                name: "IX_TestTag_testId",
                table: "TestTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSectionQuestion",
                table: "TestSectionQuestion");

            migrationBuilder.DropIndex(
                name: "IX_TestSectionQuestion_questionId",
                table: "TestSectionQuestion");

            migrationBuilder.DropIndex(
                name: "IX_TestSectionQuestion_testSectionId",
                table: "TestSectionQuestion");

            migrationBuilder.DropColumn(
                name: "testSectionQuestionid",
                table: "TestAttemptQuestionSections");

            migrationBuilder.RenameTable(
                name: "TestTag",
                newName: "TestTags");

            migrationBuilder.RenameTable(
                name: "TestSectionQuestion",
                newName: "TestSectionQuestions");

            migrationBuilder.RenameColumn(
                name: "testAttemptattemptId",
                table: "TestAttemptSections",
                newName: "testAttemptId");

            migrationBuilder.RenameColumn(
                name: "candidateid",
                table: "TestAttempts",
                newName: "candidateId");

            migrationBuilder.RenameIndex(
                name: "IX_TestAttempts_candidateid",
                table: "TestAttempts",
                newName: "IX_TestAttempts_candidateId");

            migrationBuilder.AddColumn<long>(
                name: "testQuestionId",
                table: "TestAttemptQuestionSections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestTags",
                table: "TestTags",
                column: "testTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSectionQuestions",
                table: "TestSectionQuestions",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TestAttempts_Candidates_candidateId",
                table: "TestAttempts",
                column: "candidateId",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
