using Microsoft.EntityFrameworkCore.Migrations;

namespace MovigenCore.Migrations
{
    public partial class volverint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Cantidad",
                table: "Detalle",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cantidad",
                table: "Detalle",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
