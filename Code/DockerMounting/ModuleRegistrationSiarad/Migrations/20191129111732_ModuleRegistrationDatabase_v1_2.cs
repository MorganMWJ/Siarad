using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuleRegistrationSiarad.Migrations
{
    public partial class ModuleRegistrationDatabase_v1_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_module",
                table: "module");

            migrationBuilder.RenameTable(
                name: "module",
                newName: "modules");

            migrationBuilder.AddColumn<string>(
                name: "class_code",
                table: "modules",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_modules",
                table: "modules",
                columns: new[] { "module_id", "year", "class_code" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_modules",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "class_code",
                table: "modules");

            migrationBuilder.RenameTable(
                name: "modules",
                newName: "module");

            migrationBuilder.AddPrimaryKey(
                name: "PK_module",
                table: "module",
                columns: new[] { "module_id", "year" });
        }
    }
}
