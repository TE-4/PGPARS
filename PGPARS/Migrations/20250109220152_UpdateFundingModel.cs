using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFundingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNnumber",
                table: "Fundings");

            migrationBuilder.DropIndex(
                name: "IX_Fundings_ApplicantNnumber",
                table: "Fundings");

            migrationBuilder.DropColumn(
                name: "ApplicantNnumber",
                table: "Fundings");

            migrationBuilder.AlterColumn<string>(
                name: "Nnumber",
                table: "Fundings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_Nnumber",
                table: "Fundings",
                column: "Nnumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Fundings_Applicants_Nnumber",
                table: "Fundings",
                column: "Nnumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fundings_Applicants_Nnumber",
                table: "Fundings");

            migrationBuilder.DropIndex(
                name: "IX_Fundings_Nnumber",
                table: "Fundings");

            migrationBuilder.AlterColumn<string>(
                name: "Nnumber",
                table: "Fundings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantNnumber",
                table: "Fundings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_ApplicantNnumber",
                table: "Fundings",
                column: "ApplicantNnumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Fundings_Applicants_ApplicantNnumber",
                table: "Fundings",
                column: "ApplicantNnumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber");
        }
    }
}
