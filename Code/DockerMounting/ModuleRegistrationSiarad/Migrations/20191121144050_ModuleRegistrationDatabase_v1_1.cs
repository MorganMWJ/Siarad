using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuleRegistrationSiarad.Migrations
{
    public partial class ModuleRegistrationDatabase_v1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "registered_students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "module_id",
                table: "registered_students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "year",
                table: "registered_students",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "year",
                table: "registered_students");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "registered_students",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "module_id",
                table: "registered_students",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
