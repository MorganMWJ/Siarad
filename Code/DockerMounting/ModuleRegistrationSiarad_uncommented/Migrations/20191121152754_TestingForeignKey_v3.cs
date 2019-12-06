using Microsoft.EntityFrameworkCore.Migrations;

namespace ModuleRegistrationSiarad.Migrations
{
    public partial class TestingForeignKey_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registered_students_students_user_id",
                table: "registered_students");

            migrationBuilder.DropForeignKey(
                name: "FK_registered_students_module_module_id_year",
                table: "registered_students");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_module_module_id_year",
                table: "staff");

            migrationBuilder.DropIndex(
                name: "IX_staff_module_id_year",
                table: "staff");

            migrationBuilder.DropIndex(
                name: "IX_registered_students_user_id",
                table: "registered_students");

            migrationBuilder.DropIndex(
                name: "IX_registered_students_module_id_year",
                table: "registered_students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_staff_module_id_year",
                table: "staff",
                columns: new[] { "module_id", "year" });

            migrationBuilder.CreateIndex(
                name: "IX_registered_students_user_id",
                table: "registered_students",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_registered_students_module_id_year",
                table: "registered_students",
                columns: new[] { "module_id", "year" });

            migrationBuilder.AddForeignKey(
                name: "FK_registered_students_students_user_id",
                table: "registered_students",
                column: "user_id",
                principalTable: "students",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
