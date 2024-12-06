using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantNnumber",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicantNnumber",
                table: "AspNetUsers",
                column: "ApplicantNnumber");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Applicants_ApplicantNnumber",
                table: "AspNetUsers",
                column: "ApplicantNnumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Applicants_ApplicantNnumber",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicantNnumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicantNnumber",
                table: "AspNetUsers");
        }
    }
}
