using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logic.Migrations
{
    /// <inheritdoc />
    public partial class Add_everything_related_to_synchronization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_sync_needed",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AlterColumn<long>(
                name: "CourseId",
                table: "Enrollment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<long>(
                name: "CourseId",
                table: "Disenrollment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true
            );

            migrationBuilder
                .AlterColumn<long>(
                    name: "Id",
                    table: "Courses",
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder.CreateTable(
                name: "sync",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    is_sync_required = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    row_version = table.Column<int>(
                        type: "integer",
                        nullable: false,
                        defaultValue: 1
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sync", x => x.name);
                }
            );

            migrationBuilder.Sql(
                @"create or replace function increment_row_version()
returns trigger as $$
    begin 
        new.row_version = old.row_version + 1;
        return new;
    end;
$$ language plpgsql;

create or replace trigger increment_row_version_on_update
before update on sync
for each row execute function increment_row_version();"
            );

            migrationBuilder.Sql(@"insert into sync (name) values ('Student')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from sync where name = 'Student'");
            migrationBuilder.Sql("drop trigger if exists increment_row_version_on_update on sync");
            migrationBuilder.Sql("drop function if exists increment_row_version()");

            migrationBuilder.DropTable(name: "sync");

            migrationBuilder.DropColumn(name: "is_sync_needed", table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Enrollment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint"
            );

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Disenrollment",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder
                .AlterColumn<int>(
                    name: "Id",
                    table: "Courses",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );
        }
    }
}
