using Microsoft.EntityFrameworkCore.Migrations;

namespace MovigenCore.Migrations
{
    public partial class deleteUnidadMedidadFromDetalle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Detalle");

            migrationBuilder.DropColumn(
                name: "Unidad",
                table: "Detalle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Detalle",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unidad",
                table: "Detalle",
                nullable: true);
        }
    }
}
