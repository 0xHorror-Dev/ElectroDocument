using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroDocument.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "DescSecond",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Docs");

            migrationBuilder.AddColumn<long>(
                name: "DocDetailsId",
                table: "Docs",
                type: "bigint(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reason = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Desc = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescSecond = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "FK_DocDetails_Docs",
                table: "Docs",
                column: "DocDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocDetails_Docs",
                table: "Docs",
                column: "DocDetailsId",
                principalTable: "DocDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocDetails_Docs",
                table: "Docs");

            migrationBuilder.DropTable(
                name: "DocDetails");

            migrationBuilder.DropIndex(
                name: "FK_DocDetails_Docs",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "DocDetailsId",
                table: "Docs");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DescSecond",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Docs",
                type: "text",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
