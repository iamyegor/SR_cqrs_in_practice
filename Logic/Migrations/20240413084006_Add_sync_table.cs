using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logic.Migrations
{
    /// <inheritdoc />
    public partial class Add_sync_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sync",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    is_sync_required = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: true
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

            migrationBuilder.Sql(@"insert into sync (name) values ('Student')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from sync where name = 'Student'");

            migrationBuilder.DropTable(name: "sync");
        }
    }
}
