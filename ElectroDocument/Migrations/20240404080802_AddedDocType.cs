using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class AddedDocType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<sbyte>(
                name: "DocType",
                table: "Docs",
                type: "tinyint(4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Docs",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocType",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Docs");
        }
    }
}
