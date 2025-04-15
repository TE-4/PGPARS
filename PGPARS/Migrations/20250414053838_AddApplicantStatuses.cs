using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicantStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GradAcceptStatus",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasUnfEmail",
                table: "Applicants",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnrolled",
                table: "Applicants",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkStatus",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradAcceptStatus",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "HasUnfEmail",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "IsEnrolled",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "WorkStatus",
                table: "Applicants");
        }
    }
}
