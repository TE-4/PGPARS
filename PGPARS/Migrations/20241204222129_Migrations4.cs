using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGPARS.Migrations
{
    /// <inheritdoc />
    public partial class Migrations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppSubmitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reviewer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllGPA = table.Column<double>(type: "float", nullable: true),
                    PsychGPA = table.Column<double>(type: "float", nullable: true),
                    GPAComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseReqMet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseReqComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetterQuality = table.Column<int>(type: "int", nullable: true),
                    Mentor1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mentor2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mentor3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetterComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeQuality = table.Column<int>(type: "int", nullable: true),
                    ResExpQuality = table.Column<int>(type: "int", nullable: true),
                    ResumeComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WritingSampleQuality = table.Column<int>(type: "int", nullable: true),
                    WritingSampleComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LORRelevance = table.Column<int>(type: "int", nullable: true),
                    LORQuality = table.Column<int>(type: "int", nullable: true),
                    LORComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallFitQuality = table.Column<int>(type: "int", nullable: true),
                    OverallFitComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecRec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
