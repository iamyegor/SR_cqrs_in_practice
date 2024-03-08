using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    /// <inheritdoc />
    public partial class seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment"
            );

            migrationBuilder.AlterColumn<long>(
                name: "CourseId",
                table: "Disenrollment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id"
            );

            migrationBuilder.Sql(
                @"
INSERT INTO ""Courses"" (""Name"", ""Credits"")
VALUES ('Calculus', 3),
       ('Chemistry', 3),
       ('Composition', 3),
       ('Literature', 4),
       ('Trigonometry', 4),
       ('Microeconomics', 3),
       ('Macroeconomics', 3);
"
            );

            migrationBuilder.Sql(
                @"
INSERT INTO ""Students"" (""Name"", ""Email"")
VALUES ('Alice', 'alice@gmail.com'),
       ('Bob', 'bob@outlook.com');
"
            );

            migrationBuilder.Sql(
                @"
INSERT INTO ""Enrollment"" (""StudentId"", ""CourseId"", ""Grade"")
VALUES (2, 2, 1),
       (2, 3, 3),
       (1, 1, 1),
       (1, 2, 3);
"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment"
            );

            migrationBuilder.AlterColumn<long>(
                name: "CourseId",
                table: "Disenrollment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Disenrollment_Courses_CourseId",
                table: "Disenrollment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.Sql(
                @"
DELETE FROM ""Courses""
WHERE ""Name"" IN ('Calculus', 'Chemistry', 'Composition', 'Literature', 'Trigonometry', 'Microeconomics', 'Macroeconomics');
"
            );

            migrationBuilder.Sql(
                @"
DELETE FROM ""Students""
WHERE ""Name"" IN ('Alice', 'Bob');
"
            );

            migrationBuilder.Sql(
                @"
DELETE FROM ""Enrollment""
WHERE (""StudentId"", ""CourseId"") IN ((2, 2), (2, 3), (1, 1), (1, 2));
"
            );
        }
    }
}
