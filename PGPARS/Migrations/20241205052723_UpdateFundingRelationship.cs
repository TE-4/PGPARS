using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFundingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applicant",
                table: "Fundings");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "Fundings");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantNNumber",
                table: "Fundings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_ApplicantNNumber",
                table: "Fundings",
                column: "ApplicantNNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNNumber",
                table: "Fundings",
                column: "ApplicantNNumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNNumber",
                table: "Fundings");

            migrationBuilder.DropIndex(
                name: "IX_Fundings_ApplicantNNumber",
                table: "Fundings");

            migrationBuilder.DropColumn(
                name: "ApplicantNNumber",
                table: "Fundings");

            migrationBuilder.AddColumn<string>(
                name: "Applicant",
                table: "Fundings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicantId",
                table: "Fundings",
                type: "int",
                nullable: true);
        }
    }
}
