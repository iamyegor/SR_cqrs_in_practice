using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    /// <inheritdoc />
    public partial class enablecascadedeletefordisenrollmentswhenstudentisdeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment");

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment");

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
