using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPR_BE.Migrations
{
    /// <inheritdoc />
    public partial class updatedquestionmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "candidateId",
                table: "GradedQuestions",
                newName: "candidateid");

            migrationBuilder.RenameColumn(
                name: "testInvitationId",
                table: "GradedQuestions",
                newName: "testAttempt");

            migrationBuilder.AlterColumn<long>(
                name: "questionId",
                table: "GradedQuestions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "grade",
                table: "GradedQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GradedQuestions_candidateid",
                table: "GradedQuestions",
                column: "candidateid");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedQuestions_Candidates_candidateid",
                table: "GradedQuestions",
                column: "candidateid",
                principalTable: "Candidates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedQuestions_Candidates_candidateid",
                table: "GradedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_GradedQuestions_candidateid",
                table: "GradedQuestions");

            migrationBuilder.DropColumn(
                name: "grade",
                table: "GradedQuestions");

            migrationBuilder.RenameColumn(
                name: "candidateid",
                table: "GradedQuestions",
                newName: "candidateId");

            migrationBuilder.RenameColumn(
                name: "testAttempt",
                table: "GradedQuestions",
                newName: "testInvitationId");

            migrationBuilder.AlterColumn<int>(
                name: "questionId",
                table: "GradedQuestions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
