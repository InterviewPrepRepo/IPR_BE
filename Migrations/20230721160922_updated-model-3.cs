using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPR_BE.Migrations
{
    /// <inheritdoc />
    public partial class updatedmodel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedQuestions_Candidates_candidateid",
                table: "GradedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_GradedQuestions_candidateid",
                table: "GradedQuestions");

            migrationBuilder.DropColumn(
                name: "candidateid",
                table: "GradedQuestions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "candidateid",
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
    }
}
