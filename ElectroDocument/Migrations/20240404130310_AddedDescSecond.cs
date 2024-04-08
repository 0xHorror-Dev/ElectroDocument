using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescSecond",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescSecond",
                table: "Docs");
        }
    }
}
