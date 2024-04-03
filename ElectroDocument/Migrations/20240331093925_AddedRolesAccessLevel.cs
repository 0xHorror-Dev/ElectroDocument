using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesAccessLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Policy",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "AccessLevel",
                table: "Role",
                type: "text",
                nullable: false,
                defaultValueSql: "'User'",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Employee",
                type: "bigint(20)",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "FK_Employee_Role",
                table: "Employee",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Role",
                table: "Employee",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Role",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "FK_Employee_Role",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Policy",
                table: "Employee",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValueSql: "'User'",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
