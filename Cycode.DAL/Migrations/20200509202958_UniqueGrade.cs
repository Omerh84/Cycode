using Microsoft.EntityFrameworkCore.Migrations;

namespace Cycode.DAL.Migrations
{
    public partial class UniqueGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentInCourses_CourseId",
                table: "StudentInCourses");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInCourses_CourseId_StudentId",
                table: "StudentInCourses",
                columns: new[] { "CourseId", "StudentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentInCourses_CourseId_StudentId",
                table: "StudentInCourses");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInCourses_CourseId",
                table: "StudentInCourses",
                column: "CourseId");
        }
    }
}
