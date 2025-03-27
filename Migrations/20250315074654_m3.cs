using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DrImage",
                table: "Doctors",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "DepartmentsDepartmentId",
                table: "Doctors",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_DepartmentsDepartmentId",
                table: "Doctors",
                newName: "IX_Doctors_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Doctors",
                newName: "DrImage");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Doctors",
                newName: "DepartmentsDepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                newName: "IX_Doctors_DepartmentsDepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "Department",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentsDepartmentId",
                table: "Doctors",
                column: "DepartmentsDepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
