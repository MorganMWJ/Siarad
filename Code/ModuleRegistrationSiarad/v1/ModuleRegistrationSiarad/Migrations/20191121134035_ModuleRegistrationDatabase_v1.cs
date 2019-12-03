using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuleRegistrationSiarad.Migrations
{
    public partial class ModuleRegistrationDatabase_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegisteredStudents",
                table: "RegisteredStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleTitle",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "students");

            migrationBuilder.RenameTable(
                name: "Staffs",
                newName: "staff");

            migrationBuilder.RenameTable(
                name: "RegisteredStudents",
                newName: "registered_students");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "module");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "students",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Forename",
                table: "students",
                newName: "forename");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "students",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "AutoId",
                table: "staff",
                newName: "auto_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "registered_students",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "registered_students",
                newName: "module_id");

            migrationBuilder.RenameColumn(
                name: "AutoId",
                table: "registered_students",
                newName: "auto_id");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "module",
                newName: "module_title");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "forename",
                table: "students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "module_id",
                table: "staff",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "staff_id",
                table: "staff",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "year",
                table: "module",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "module_id",
                table: "module",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "coordinator_id",
                table: "module",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staff",
                table: "staff",
                column: "auto_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_registered_students",
                table: "registered_students",
                column: "auto_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_module",
                table: "module",
                columns: new[] { "year", "module_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_staff",
                table: "staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_registered_students",
                table: "registered_students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_module",
                table: "module");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "staff");

            migrationBuilder.DropColumn(
                name: "year",
                table: "module");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "module");

            migrationBuilder.DropColumn(
                name: "coordinator_id",
                table: "module");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "staff",
                newName: "Staffs");

            migrationBuilder.RenameTable(
                name: "registered_students",
                newName: "RegisteredStudents");

            migrationBuilder.RenameTable(
                name: "module",
                newName: "Modules");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "Students",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "forename",
                table: "Students",
                newName: "Forename");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Students",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "auto_id",
                table: "Staffs",
                newName: "AutoId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "RegisteredStudents",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "module_id",
                table: "RegisteredStudents",
                newName: "ModuleId");

            migrationBuilder.RenameColumn(
                name: "auto_id",
                table: "RegisteredStudents",
                newName: "AutoId");

            migrationBuilder.RenameColumn(
                name: "module_title",
                table: "Modules",
                newName: "StaffId");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Students",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Forename",
                table: "Students",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                table: "Modules",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModuleTitle",
                table: "Modules",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs",
                column: "AutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegisteredStudents",
                table: "RegisteredStudents",
                column: "AutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "ModuleId");
        }
    }
}
