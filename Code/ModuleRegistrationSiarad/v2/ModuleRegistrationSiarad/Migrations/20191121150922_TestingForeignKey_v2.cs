using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuleRegistrationSiarad.Migrations
{
    public partial class TestingForeignKey_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registered_students_module_module_id_year",
                table: "registered_students");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_module_module_id_year",
                table: "staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_module",
                table: "module");

            migrationBuilder.AddPrimaryKey(
                name: "PK_module",
                table: "module",
                columns: new[] { "module_id", "year" });

            migrationBuilder.AddForeignKey(
                name: "FK_registered_students_module_module_id_year",
                table: "registered_students",
                columns: new[] { "module_id", "year" },
                principalTable: "module",
                principalColumns: new[] { "module_id", "year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_module_module_id_year",
                table: "staff",
                columns: new[] { "module_id", "year" },
                principalTable: "module",
                principalColumns: new[] { "module_id", "year" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registered_students_module_module_id_year",
                table: "registered_students");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_module_module_id_year",
                table: "staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_module",
                table: "module");

            migrationBuilder.AddPrimaryKey(
                name: "PK_module",
                table: "module",
                columns: new[] { "year", "module_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_registered_students_module_module_id_year",
                table: "registered_students",
                columns: new[] { "module_id", "year" },
                principalTable: "module",
                principalColumns: new[] { "year", "module_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_module_module_id_year",
                table: "staff",
                columns: new[] { "module_id", "year" },
                principalTable: "module",
                principalColumns: new[] { "year", "module_id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
