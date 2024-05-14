using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class DocsVersionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Number",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Individual",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValueSql: "''",
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "''")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Individual",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "Individual",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValueSql: "''",
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "''")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Individual",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<short>(
                name: "Notified",
                table: "Docs",
                type: "smallint(6)",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<long>(
                name: "Responsible",
                table: "Docs",
                type: "bigint(20)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ResponsibleNotified",
                table: "Docs",
                type: "smallint(6)",
                nullable: true,
                defaultValueSql: "'0'");

            migrationBuilder.CreateTable(
                name: "DocumentVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    DocId = table.Column<long>(type: "bigint(20)", nullable: false),
                    NewDocId = table.Column<long>(type: "bigint(20)", nullable: true),
                    DocIdSrc = table.Column<long>(type: "bigint(20)", nullable: false),
                    EditorId = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK1_DocId_Docs_Id",
                        column: x => x.DocId,
                        principalTable: "Docs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK2_NewDocId_Docs_Id",
                        column: x => x.NewDocId,
                        principalTable: "Docs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK3_DocIdSrc_Docs_Id",
                        column: x => x.DocIdSrc,
                        principalTable: "Docs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK4_EditorId_Employee_Id",
                        column: x => x.EditorId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "FK_DocsNew_Responsible",
                table: "Docs",
                column: "Responsible");

            migrationBuilder.CreateIndex(
                name: "FK1_DocId_Docs_Id",
                table: "DocumentVersion",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "FK2_NewDocId_Docs_Id",
                table: "DocumentVersion",
                column: "NewDocId");

            migrationBuilder.CreateIndex(
                name: "FK3_DocIdSrc_Docs_Id",
                table: "DocumentVersion",
                column: "DocIdSrc");

            migrationBuilder.CreateIndex(
                name: "FK4_EditorId_Employee_Id",
                table: "DocumentVersion",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocsNew_Responsible",
                table: "Docs",
                column: "Responsible",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocsNew_Responsible",
                table: "Docs");

            migrationBuilder.DropTable(
                name: "DocumentVersion");

            migrationBuilder.DropIndex(
                name: "FK_DocsNew_Responsible",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Notified",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Responsible",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "ResponsibleNotified",
                table: "Docs");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Individual",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "''",
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldDefaultValueSql: "''")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Individual",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "Individual",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "''",
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldDefaultValueSql: "''")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Individual",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Employee",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "Number",
                table: "Docs",
                column: "Number",
                unique: true);
        }
    }
}
