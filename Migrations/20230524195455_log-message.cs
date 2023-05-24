using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPR_BE.Migrations
{
    /// <inheritdoc />
    public partial class logmessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "InterviewBotLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "message",
                table: "InterviewBotLogs");
        }
    }
}
