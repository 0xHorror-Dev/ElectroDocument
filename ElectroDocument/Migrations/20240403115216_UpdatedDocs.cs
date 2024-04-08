using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "descFirst",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "descSecond",
                table: "Docs");

            migrationBuilder.RenameColumn(
                name: "SecondDay",
                table: "Docs",
                newName: "DateThird");

            migrationBuilder.RenameColumn(
                name: "FirstDay",
                table: "Docs",
                newName: "DateSecond");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Docs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Docs",
                type: "bigint(20)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Number",
                table: "Docs",
                type: "bigint(20)",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Sum",
                table: "Docs",
                type: "int(11)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "FK_DocsNew_Employee",
                table: "Docs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "Number",
                table: "Docs",
                column: "Number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocsNew_Employee",
                table: "Docs",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocsNew_Employee",
                table: "Docs");

            migrationBuilder.DropIndex(
                name: "FK_DocsNew_Employee",
                table: "Docs");

            migrationBuilder.DropIndex(
                name: "Number",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Sum",
                table: "Docs");

            migrationBuilder.RenameColumn(
                name: "DateThird",
                table: "Docs",
                newName: "SecondDay");

            migrationBuilder.RenameColumn(
                name: "DateSecond",
                table: "Docs",
                newName: "FirstDay");

            migrationBuilder.AddColumn<sbyte>(
                name: "Type",
                table: "Docs",
                type: "tinyint(4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "descFirst",
                table: "Docs",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "descSecond",
                table: "Docs",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Document = table.Column<long>(type: "bigint(20)", nullable: true, defaultValueSql: "'0'"),
                    Employee = table.Column<long>(type: "bigint(20)", nullable: false),
                    Owner = table.Column<long>(type: "bigint(20)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Desc = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false, defaultValueSql: "'0'", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
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
    }
}
