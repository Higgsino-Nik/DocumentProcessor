using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentProcessor.Migrations
{
    /// <inheritdoc />
    public partial class DocumentCreationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Document",
                type: "DATETIME",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(table: "Document", name: "CreatedOnUtc");
        }
    }
}
