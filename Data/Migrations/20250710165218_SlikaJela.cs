using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestoranASP.Data.Migrations
{
    /// <inheritdoc />
    public partial class SlikaJela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slika",
                table: "Jela",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slika",
                table: "Jela");
        }
    }
}
