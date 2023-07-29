using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BymseRead.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToBookmarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorCode",
                table: "Bookmarks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Bookmarks");
        }
    }
}
