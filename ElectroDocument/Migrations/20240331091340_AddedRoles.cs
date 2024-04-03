using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Policy",
                keyValue: null,
                column: "Policy",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Policy",
                table: "Employee",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValueSql: "'User'",
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Docs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    FirstDay = table.Column<DateOnly>(type: "date", nullable: true),
                    SecondDay = table.Column<DateOnly>(type: "date", nullable: true),
                    descFirst = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descSecond = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Desc = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false, defaultValueSql: "'0'", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Document = table.Column<long>(type: "bigint(20)", nullable: true, defaultValueSql: "'0'"),
                    Owner = table.Column<long>(type: "bigint(20)", nullable: false),
                    Employee = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK1_History_Document",
                        column: x => x.Document,
                        principalTable: "Docs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK2_History_Owner",
                        column: x => x.Owner,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK3_History_Employee",
                        column: x => x.Employee,
                        principalTable: "Employee",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "FK1_History_Document",
                table: "History",
                column: "Document");

            migrationBuilder.CreateIndex(
                name: "FK2_History_Owner",
                table: "History",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "FK3_History_Employee",
                table: "History",
                column: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Docs");

            migrationBuilder.AlterColumn<string>(
                name: "Policy",
                table: "Employee",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldDefaultValueSql: "'User'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");
        }
    }
}
