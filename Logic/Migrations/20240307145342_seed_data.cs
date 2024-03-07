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
        INSERT INTO ""Courses"" (""Id"", ""Name"", ""Credits"")
        VALUES (1, 'Calculus', 3),
               (2, 'Chemistry', 3),
               (3, 'Composition', 3),
               (4, 'Literature', 4),
               (5, 'Trigonometry', 4),
               (6, 'Microeconomics', 3),
               (7, 'Macroeconomics', 3);
    "
            );

            // Students data insertion
            migrationBuilder.Sql(
                @"
        INSERT INTO ""Students"" (""Id"", ""Name"", ""Email"")
        VALUES (1, 'Alice', 'alice@gmail.com'),
               (2, 'Bob', 'bob@outlook.com');
    "
            );

            // Enrollment data insertion
            migrationBuilder.Sql(
                @"
        INSERT INTO ""Enrollment"" (""Id"", ""StudentId"", ""CourseId"", ""Grade"")
        VALUES (5, 2, 2, 1),
               (13, 2, 3, 3),
               (20, 1, 1, 1),
               (38, 1, 2, 3);
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
        DELETE FROM ""Enrollment""
        WHERE ""Id"" IN (5, 13, 20, 38);
    "
            );

            // Delete specific Students data
            migrationBuilder.Sql(
                @"
        DELETE FROM ""Students""
        WHERE ""Id"" IN (1, 2);
    "
            );

            // Delete specific Courses data
            migrationBuilder.Sql(
                @"
        DELETE FROM ""Courses""
        WHERE ""Id"" IN (1, 2, 3, 4, 5, 6, 7);
    "
            );
        }
    }
}
