using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    /// <inheritdoc />
    public partial class Lowercase_all_column_and_table_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Courses_CourseId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disenrollment",
                table: "Disenrollment");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "students");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "courses");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                newName: "enrollments");

            migrationBuilder.RenameTable(
                name: "Disenrollment",
                newName: "disenrollments");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "students",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "students",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "students",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "courses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "courses",
                newName: "credits");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "courses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "enrollments",
                newName: "grade");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "enrollments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "enrollments",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "enrollments",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentId",
                table: "enrollments",
                newName: "IX_enrollments_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseId",
                table: "enrollments",
                newName: "IX_enrollments_course_id");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "disenrollments",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "disenrollments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "disenrollments",
                newName: "date_time");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "disenrollments",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "disenrollments",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "IX_Disenrollment_StudentId",
                table: "disenrollments",
                newName: "IX_disenrollments_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Disenrollment_CourseId",
                table: "disenrollments",
                newName: "IX_disenrollments_course_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_enrollments",
                table: "enrollments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_disenrollments",
                table: "disenrollments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_disenrollments_courses_course_id",
                table: "disenrollments",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_disenrollments_students_student_id",
                table: "disenrollments",
                column: "student_id",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_courses_course_id",
                table: "enrollments",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_students_student_id",
                table: "enrollments",
                column: "student_id",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_disenrollments_courses_course_id",
                table: "disenrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_disenrollments_students_student_id",
                table: "disenrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_courses_course_id",
                table: "enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_students_student_id",
                table: "enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_enrollments",
                table: "enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_disenrollments",
                table: "disenrollments");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "enrollments",
                newName: "Enrollment");

            migrationBuilder.RenameTable(
                name: "disenrollments",
                newName: "Disenrollment");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Students",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Courses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "credits",
                table: "Courses",
                newName: "Credits");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Courses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Enrollment",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Enrollment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "Enrollment",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "Enrollment",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_student_id",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_course_id",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseId");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Disenrollment",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Disenrollment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date_time",
                table: "Disenrollment",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "Disenrollment",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "Disenrollment",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_disenrollments_student_id",
                table: "Disenrollment",
                newName: "IX_Disenrollment_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_disenrollments_course_id",
                table: "Disenrollment",
                newName: "IX_Disenrollment_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disenrollment",
                table: "Disenrollment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Students_StudentId",
                table: "Disenrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Courses_CourseId",
                table: "Enrollment",
                column: "CourseId",
                principalTable: "Courses",
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
    }
}
