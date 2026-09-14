using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTCGStore.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDescripcionIlustrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Cartas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ilustrador",
                table: "Cartas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Cartas");

            migrationBuilder.DropColumn(
                name: "Ilustrador",
                table: "Cartas");
        }
    }
}
