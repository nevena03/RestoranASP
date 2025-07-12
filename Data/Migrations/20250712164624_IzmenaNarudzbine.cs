using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestoranASP.Data.Migrations
{
    /// <inheritdoc />
    public partial class IzmenaNarudzbine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kolicina",
                table: "StavkeNarudzbina",
                newName: "BrojPorcija");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrojPorcija",
                table: "StavkeNarudzbina",
                newName: "Kolicina");
        }
    }
}
