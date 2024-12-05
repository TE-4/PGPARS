using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class RenameApplicantNNumberColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNNumber",
                table: "Fundings");

            migrationBuilder.RenameColumn(
                name: "ApplicantNNumber",
                table: "Fundings",
                newName: "Nnumber");

            migrationBuilder.RenameIndex(
                name: "IX_Fundings_ApplicantNNumber",
                table: "Fundings",
                newName: "IX_Fundings_Nnumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Fundings_Applicants_Nnumber",
                table: "Fundings",
                column: "Nnumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fundings_Applicants_Nnumber",
                table: "Fundings");

            migrationBuilder.RenameColumn(
                name: "Nnumber",
                table: "Fundings",
                newName: "ApplicantNNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Fundings_Nnumber",
                table: "Fundings",
                newName: "IX_Fundings_ApplicantNNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNNumber",
                table: "Fundings",
                column: "ApplicantNNumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
