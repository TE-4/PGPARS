using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFundingApplicantModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nnumber",
                table: "Fundings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nnumber",
                table: "Fundings");
        }
    }
}
