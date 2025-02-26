using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class newApplicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Applicants_NNumber",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "NNumber",
                table: "Reviews",
                newName: "Nnumber");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_NNumber",
                table: "Reviews",
                newName: "IX_Reviews_Nnumber");

            migrationBuilder.AddColumn<string>(
                name: "CitizenshipStatus",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MissingItems",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReviews",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCitizenship",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Program",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "School1GPA",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School1Institution",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School1Major",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "School2GPA",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School2Institution",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School2Major",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "School3GPA",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School3Institution",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School3Major",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "School4GPA",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School4Institution",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School4Major",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "School5GPA",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School5Institution",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School5Major",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Applicants_Nnumber",
                table: "Reviews",
                column: "Nnumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Applicants_Nnumber",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CitizenshipStatus",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "MissingItems",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "NumberOfReviews",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "PrimaryCitizenship",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School1GPA",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School1Institution",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School1Major",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School2GPA",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School2Institution",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School2Major",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School3GPA",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School3Institution",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School3Major",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School4GPA",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School4Institution",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School4Major",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School5GPA",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School5Institution",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "School5Major",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "Nnumber",
                table: "Reviews",
                newName: "NNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_Nnumber",
                table: "Reviews",
                newName: "IX_Reviews_NNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Applicants_NNumber",
                table: "Reviews",
                column: "NNumber",
                principalTable: "Applicants",
                principalColumn: "Nnumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
